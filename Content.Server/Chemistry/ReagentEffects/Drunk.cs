using Content.Server.Traits.Assorted;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Drunk;
using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.ReagentEffects;

public sealed partial class Drunk : ReagentEffect
{
    /// <summary>
    ///     BoozePower is how long each metabolism cycle will make the drunk effect last for.
    /// </summary>
    [DataField]
    public float BoozePower = 3f;

    /// <summary>
    ///     Whether speech should be slurred.
    /// </summary>
    [DataField]
    public bool SlurSpeech = true;

    /// <summary>
    ///     Whether this effect comes from actual drunkenness from alcohol.
    /// </summary>
    [DataField]
    public bool IsFromAlcohol = false;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-drunk", ("chance", Probability));

    public override void Effect(ReagentEffectArgs args)
    {
        var boozePower = BoozePower;

        boozePower *= Math.Max(args.Scale, 1);

        var drunkSys = args.EntityManager.EntitySysManager.GetEntitySystem<SharedDrunkSystem>();
        drunkSys.TryApplyDrunkenness(args.SolutionEntity, boozePower, SlurSpeech);

        if (args.EntityManager.TryGetComponent(args.SolutionEntity, out DrunkenResilienceComponent? resilience))
            EntitySystem.Get<DrunkenResilienceSystem>().AddDrunkenness(args.SolutionEntity, boozePower, resilience);
    }
}
