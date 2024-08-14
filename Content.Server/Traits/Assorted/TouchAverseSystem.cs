using Content.Server.Mood;
using Content.Shared.Buckle.Components;
using Content.Shared.Mood;
using Content.Shared.Movement.Pulling.Events;

namespace Content.Server.Traits.Assorted;

public sealed class TouchAverseSystem : EntitySystem
{
    [Dependency] private readonly MoodSystem _mood = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TouchAverseComponent, PullStartedMessage>(OnPulled);
        SubscribeLocalEvent<TouchAverseComponent, BuckleChangeEvent>(OnForceBuckleChange);
    }

    private void OnPulled(EntityUid uid, TouchAverseComponent comp, ref PullStartedMessage args)
    {
        var ev = new MoodEffectEvent("TouchAversePulled");
        RaiseLocalEvent(uid, ev);
    }

    private void OnForceBuckleChange(EntityUid uid, TouchAverseComponent comp, ref BuckleChangeEvent args)
    {
        if (args.BuckledEntity == args.BucklingEntity)
            return;

        if (args.Buckling)
        {
            var ev = new MoodEffectEvent("TouchAverseBuckled");
            RaiseLocalEvent(uid, ev);
        }
        else
        {
            var ev = new MoodEffectEvent("TouchAverseUnbuckled");
            RaiseLocalEvent(uid, ev);
        }
    }
}
