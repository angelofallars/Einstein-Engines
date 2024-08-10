using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Traits.Assorted.Components;

namespace Content.Shared.Traits.Assorted.Systems;

public sealed class HungerModifierSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<HungerModifierComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, HungerModifierComponent component, ComponentStartup args)
    {
        if (!TryComp<HungerComponent>(uid, out var hunger))
            return;

        hunger.BaseDecayRate *= component.DecayRateMultiplier;
    }
}
