using Content.Shared.Damage;
using Content.Shared.Weapons.Melee;

namespace Content.Server.Traits.Assorted;

public sealed class TraitMeleeModifierSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TraitMeleeModifierComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, TraitMeleeModifierComponent component, ComponentStartup args)
    {
        if (!TryComp<MeleeWeaponComponent>(uid, out var meleeComponent))
            return;

        if (component.Damage != null)
            meleeComponent.Damage = component.Damage;

        if (component.SoundHit != null)
            meleeComponent.HitSound = component.SoundHit;

        if (component.Animation != "")
            meleeComponent.Animation = component.Animation;

        if (component.WideAnimation != "")
            meleeComponent.WideAnimation = component.WideAnimation;

        if (component.AttackRate != null)
            meleeComponent.AttackRate = (float) component.AttackRate;
    }
}
