using Content.Shared.Administration.Logs;
using Content.Shared.Camera;
using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Events;
using Content.Shared.Database;
using Content.Shared.Effects;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Projectiles;
using Content.Shared.Popups;
using Content.Shared.Throwing;
using Content.Shared.Weapons.Melee;
using Robust.Shared.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Damage.Systems
{
    public abstract partial class SharedDamageOtherOnHitSystem : EntitySystem
    {
        [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
        [Dependency] private readonly DamageableSystem _damageable = default!;
        [Dependency] private readonly SharedCameraRecoilSystem _sharedCameraRecoil = default!;
        [Dependency] private readonly SharedColorFlashEffectSystem _color = default!;
        [Dependency] private readonly ThrownItemSystem _thrownItem = default!;
        [Dependency] private readonly SharedPhysicsSystem _physics = default!;
        [Dependency] private readonly MeleeSoundSystem _meleeSound = default!;
        [Dependency] private readonly IPrototypeManager _protoManager = default!;

        public override void Initialize()
        {
            SubscribeLocalEvent<DamageOtherOnHitComponent, ComponentStartup>(OnStartup);
            SubscribeLocalEvent<DamageOtherOnHitComponent, ThrowDoHitEvent>(OnDoHit);
            SubscribeLocalEvent<DamageOtherOnHitComponent, ItemToggledEvent>(OnItemToggle);
            SubscribeLocalEvent<DamageOtherOnHitComponent, ThrownEvent>(OnThrown);
        }

        /// <summary>
        ///   Inherit stats from MeleeWeapon.
        /// </summary>
        private void OnStartup(EntityUid uid, DamageOtherOnHitComponent component, ComponentStartup args)
        {
            if (!component.InheritMeleeStats || !TryComp<MeleeWeaponComponent>(uid, out var melee))
                return;

            if (component.Damage == null)
                component.Damage = melee.Damage;
            if (component.SoundHit == null)
                component.SoundHit = melee.SoundHit;
            if (component.SoundNoDamage == null)
                component.SoundNoDamage = melee.SoundNoDamage;
        }


        private void OnDoHit(EntityUid uid, DamageOtherOnHitComponent component, ThrowDoHitEvent args)
        {
            if (component.HitQuantity >= component.MaxHitQuantity)
                return;

            TryComp<MeleeWeaponComponent>(uid, out var melee);

            var modifiedDamage = _damageable.TryChangeDamage(args.Target, component.Damage ?? new DamageSpecifier(), component.IgnoreResistances, origin: args.Component.Thrower);

            // Log damage only for mobs. Useful for when people throw spears at each other, but also avoids log-spam when explosions send glass shards flying.
            if (modifiedDamage != null)
            {
                if (HasComp<MobStateComponent>(args.Target))
                    _adminLogger.Add(LogType.ThrowHit, $"{ToPrettyString(args.Target):target} received {modifiedDamage.GetTotal():damage} damage from collision");

                _meleeSound.PlayHitSound(args.Target, null, SharedMeleeWeaponSystem.GetHighestDamageSound(modifiedDamage, _protoManager), null,
                    component.SoundHit, component.SoundNoDamage);
            }

            if (modifiedDamage is { Empty: false })
            {
                _color.RaiseEffect(Color.Red, new List<EntityUid>() { args.Target }, Filter.Pvs(args.Target, entityManager: EntityManager));
            }

            if (TryComp<PhysicsComponent>(uid, out var body) && body.LinearVelocity.LengthSquared() > 0f)
            {
                var direction = body.LinearVelocity.Normalized();
                _sharedCameraRecoil.KickCamera(args.Target, direction);
            }

            // TODO: If more stuff touches this then handle it after.
            if (TryComp<PhysicsComponent>(uid, out var physics))
            {
                _thrownItem.LandComponent(args.Thrown, args.Component, physics, false);

                if (!HasComp<EmbeddableProjectileComponent>(args.Thrown))
                {
                    var newVelocity = physics.LinearVelocity;
                    newVelocity.X = -newVelocity.X / 4;
                    newVelocity.Y = -newVelocity.Y / 4;
                    _physics.SetLinearVelocity(uid, newVelocity, body: physics);
                }
            }

            component.HitQuantity += 1;
        }

        /// <summary>
        ///   Used to update the DamageOtherOnHit component on item toggle.
        /// </summary>
        private void OnItemToggle(EntityUid uid, DamageOtherOnHitComponent component, ItemToggledEvent args)
        {
            if (!component.InheritMeleeStats || !TryComp<ItemToggleMeleeWeaponComponent>(uid, out var itemToggleMelee))
                return;

            if (args.Activated)
            {
                if (itemToggleMelee.ActivatedDamage != null)
                {
                    component.DeactivatedDamage ??= component.Damage;
                    component.Damage = itemToggleMelee.ActivatedDamage;
                }

                component.DeactivatedSoundHit = component.SoundHit;
                component.SoundHit = itemToggleMelee.ActivatedSoundOnHit;

                if (itemToggleMelee.ActivatedSoundOnHitNoDamage != null)
                {
                    component.DeactivatedSoundNoDamage ??= component.SoundNoDamage;
                    component.SoundNoDamage = itemToggleMelee.ActivatedSoundOnHitNoDamage;
                }
            }
            else
            {
                if (component.DeactivatedDamage != null)
                    component.Damage = component.DeactivatedDamage;

                component.SoundHit = component.DeactivatedSoundHit;

                if (component.DeactivatedSoundNoDamage != null)
                    component.SoundNoDamage = component.DeactivatedSoundNoDamage;
            }
        }

        private void OnThrown(EntityUid uid, DamageOtherOnHitComponent component, ThrownEvent args)
        {
            component.HitQuantity = 0;
        }
    }
}
