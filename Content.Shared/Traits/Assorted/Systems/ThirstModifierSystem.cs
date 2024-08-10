using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Traits.Assorted.Components;

namespace Content.Shared.Traits.Assorted.Systems;

public sealed class ThirstModifierSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ThirstModifierComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, ThirstModifierComponent component, ComponentStartup args)
    {
        if (!TryComp<ThirstComponent>(uid, out var thirst))
            return;

        thirst.BaseDecayRate *= component.DecayRateMultiplier;
    }
}
