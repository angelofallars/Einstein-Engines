using Content.Shared.Chat.TypingIndicator;
using Content.Shared.Speech;
using Content.Shared.Popups;
using Robust.Shared.Timing;

namespace Content.Shared.Speech.EntitySystems;

public sealed partial class SpeechModifierSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpeechModifierComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<SpeechModifierComponent, ComponentRemove>(OnRemove);
    }

    private void OnStartup(EntityUid uid, SpeechModifierComponent component, ComponentStartup args)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        if (TryComp<SpeechComponent>(uid, out var speech))
        {
            component.OriginalSpeechSounds = speech.SpeechSounds;
            component.OriginalSpeechVerb = speech.SpeechVerb;

            if (component.SpeechSounds is {} speechSounds)
            {
                speech.SpeechSounds = speechSounds;
            }

            if (component.SpeechVerb is {} speechVerb)
            {
                speech.SpeechVerb = speechVerb;
            }
        }

        var typing = TryComp<TypingIndicatorComponent>(uid, out var comp)
            ? comp
            : EnsureComp<TypingIndicatorComponent>(uid);

        component.OriginalTypingIndicator = typing.Prototype;

        typing.Prototype = component.TypingIndicator ?? typing.Prototype;
        Dirty(uid, typing);
    }

    private void OnRemove(EntityUid uid, SpeechModifierComponent component, ComponentRemove args)
    {
        if (TryComp<SpeechComponent>(uid, out var speech))
        {
            speech.SpeechSounds = component.OriginalSpeechSounds;

            if (component.OriginalSpeechVerb is {} originalSpeechVerb)
                speech.SpeechVerb = originalSpeechVerb;
        }


        if (TryComp<TypingIndicatorComponent>(uid, out var typing) &&
            component.OriginalTypingIndicator is string originalTypingPrototype)
        {
            typing.Prototype = originalTypingPrototype;
            Dirty(uid, typing);
        }
    }
}
