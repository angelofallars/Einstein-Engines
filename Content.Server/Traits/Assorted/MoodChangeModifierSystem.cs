using Content.Server.Mood;
using Content.Shared.Mood;

namespace Content.Server.Traits.Assorted;

public sealed class MoodChangeModifierSystem : EntitySystem
{
    [Dependency] private readonly MoodSystem _mood = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MoodChangeModifierComponent, ComponentStartup>(OnMoodStartup);
        SubscribeLocalEvent<MoodChangeModifierComponent, OnSetMoodEvent>(OnSetMood);
    }

    private void OnMoodStartup(EntityUid uid, MoodChangeModifierComponent component, ComponentStartup args)
    {
        if (!TryComp<MoodComponent>(uid, out var mood))
            return;

        // _mood.ReapplyAllEffects(uid, mood);
    }

    private void OnSetMood(EntityUid uid, MoodChangeModifierComponent comp, ref OnSetMoodEvent args)
    {
        args.MoodChangedAmount *= comp.Multiplier;
    }
}
