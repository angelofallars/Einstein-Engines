using Content.Shared.CombatMode.Pacification;
using Content.Shared.Damage.Events;
using Content.Shared.FixedPoint;
using Content.Shared.Interaction.Events;
using Content.Shared.Traits.Assorted.Components;
using Content.Shared.Popups;
using Content.Shared.Weapons.Melee;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.Traits.Assorted;

public sealed class MonkSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly PacificationSystem _pacification = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MonkComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<MonkComponent, MeleeHitEvent>(OnMeleeHit);
        SubscribeLocalEvent<MonkComponent, AttackAttemptEvent>(OnAttackAttempt);
        SubscribeLocalEvent<MonkComponent, ShotAttemptedEvent>(OnShootAttempt);
    }

    private void OnInit(EntityUid uid, MonkComponent component, ComponentInit args)
    {
        if (!TryComp<MeleeWeaponComponent>(uid, out var melee))
            return;

        melee.Range *= component.MeleeRangeModifier;
        melee.AttackRate *= component.AttackRateModifier;
    }

    private void OnMeleeHit(EntityUid uid, MonkComponent component, MeleeHitEvent args)
    {
        args.ModifiersList.Add(component.MeleeModifiers);
        args.BonusDamage = component.MeleeFlatBonuses;
    }

    private void OnAttackAttempt(EntityUid uid, MonkComponent component, AttackAttemptEvent args)
    {
        // If it's an unarmed attack let it go through
        if (args.Weapon != null && args.Weapon.Value.Owner == uid)
            return;

        // If it's a disarm, let it go through
        if (args.Disarm)
            return;

        // Allow attacking with no target. This should be fine.
        // If it's a wide swing, that will be handled with a later AttackAttemptEvent raise.
        if (args.Target == null)
            return;

        // If we would do zero damage, it should be fine.
        if (args.Weapon != null && args.Weapon.Value.Comp.Damage.GetTotal() == FixedPoint2.Zero)
            return;

        // Use the pacification system to allow structural attacks
        if (_pacification.PacifiedCanAttack(uid, args.Target.Value, out var _))
            return;

        ShowPopup((uid, component), args.Target.Value, "monk-cannot-use-weapon");
        args.Cancel();
    }

    private void OnShootAttempt(Entity<MonkComponent> ent, ref ShotAttemptedEvent args)
    {
        ShowPopup(ent, args.Used, "monk-cannot-use-weapon");
        args.Cancel();
    }

    private void ShowPopup(Entity<MonkComponent> user, EntityUid target, string reason)
    {
        // Popup logic.
        // Cooldown is needed because the input events for melee/shooting etc. will fire continuously
        if (target == user.Comp.LastAttackedEntity
            && !(_timing.CurTime > user.Comp.NextPopupTime))
            return;

        _popup.PopupClient(Loc.GetString(reason), user, user);
        user.Comp.NextPopupTime = _timing.CurTime + user.Comp.PopupCooldown;
        user.Comp.LastAttackedEntity = target;
    }
}
