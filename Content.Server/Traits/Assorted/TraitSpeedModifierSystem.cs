using Content.Shared.Movement.Systems;
using Content.Server.Traits.Assorted;

namespace Content.Shared.Traits.Assorted;

public sealed class TraitSpeedModifierSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TraitSpeedModifierComponent, RefreshMovementSpeedModifiersEvent>(OnRefreshMovementSpeed);
        SubscribeLocalEvent<TraitSpeedModifierComponent, OnStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, TraitSpeedModifierComponent component, ComponentStartup args)
    {
        if (!TryComp<TraitSpeedModifierComponent>(uid, out var threshold))
            return;

        var critThreshold = _threshold.GetThresholdForState(uid, Mobs.MobState.Critical, threshold);
        if (critThreshold != 0)
            _threshold.SetMobStateThreshold(uid, critThreshold + component.CritThresholdModifier, Mobs.MobState.Critical);
    }

    private void OnRefreshMovementSpeed(EntityUid uid, TraitSpeedModifierComponent component, RefreshMovementSpeedModifiersEvent args)
    {
        args.ModifySpeed(component.WalkModifier, component.SprintModifier);
    }
}
