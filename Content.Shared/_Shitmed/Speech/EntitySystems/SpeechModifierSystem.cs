using Content.Shared._Shitmed.Speech.Components;
using Content.Shared.Chat.TypingIndicator;
using Content.Shared.Speech;
using Content.Shared.Popups;
using Robust.Shared.Timing;

namespace Content.Shared._Shitmed.Speech.EntitySystems;

public sealed partial class SpeechModifierSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpeechModifierComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<SpeechModifierComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnStartup(EntityUid uid, SpeechModifierComponent component, ComponentStartup args)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        if (!TryComp<SpeechComponent>(uid, out var speech))
            return;

        component.OriginalSpeechSounds = speech.SpeechSounds;
        if (component.SpeechSounds is {} speechSounds)
            speech.SpeechSounds = speechSounds;

        component.OriginalSpeechVerb = speech.SpeechVerb;
        if (component.SpeechVerb is {} speechVerb)
            speech.SpeechVerb = speechVerb;
    }

    private void OnShutdown(EntityUid uid, SpeechModifierComponent component, ComponentShutdown args)
    {
        if (!TryComp<SpeechComponent>(uid, out var speech))
            return;

        speech.SpeechSounds = component.OriginalSpeechSounds;
        if (component.OriginalSpeechVerb is {} originalSpeechVerb)
            speech.SpeechVerb = originalSpeechVerb;
    }
}
