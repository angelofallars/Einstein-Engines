using Content.Shared._Shitmed.Speech.Components; // Shitmed Change
using Robust.Shared.Timing; // Shitmed Change

namespace Content.Shared.Speech
{
    public sealed class SpeechSystem : EntitySystem
    {
        [Dependency] private readonly IGameTiming _timing = default!; // Shitmed Change

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<SpeakAttemptEvent>(OnSpeakAttempt);

            // Shitmed Change Start
            SubscribeLocalEvent<SpeechModifierComponent, ComponentStartup>(OnModifierStartup);
            SubscribeLocalEvent<SpeechModifierComponent, ComponentShutdown>(OnModifierShutdown);
            // Shitmed Change End
        }

        public void SetSpeech(EntityUid uid, bool value, SpeechComponent? component = null)
        {
            if (value && !Resolve(uid, ref component))
                return;

            component = EnsureComp<SpeechComponent>(uid);

            if (component.Enabled == value)
                return;

            component.Enabled = value;

            Dirty(uid, component);
        }

        private void OnSpeakAttempt(SpeakAttemptEvent args)
        {
            if (!TryComp(args.Uid, out SpeechComponent? speech) || !speech.Enabled)
                args.Cancel();
        }

        // Shitmed Change Start
        private void OnModifierStartup(EntityUid uid, SpeechModifierComponent component, ComponentStartup args)
        {
            if (!_timing.IsFirstTimePredicted || !TryComp<SpeechComponent>(uid, out var speech))
                return;

            component.OriginalSpeechSounds = speech.SpeechSounds;
            speech.SpeechSounds = component.SpeechSounds;

            component.OriginalSpeechVerb = speech.SpeechVerb;
            speech.SpeechVerb = component.SpeechVerb;

            component.OriginalAllowedEmotes = speech.AllowedEmotes;
            speech.AllowedEmotes = component.AllowedEmotes;
        }

        private void OnModifierShutdown(EntityUid uid, SpeechModifierComponent component, ComponentShutdown args)
        {
            if (!TryComp<SpeechComponent>(uid, out var speech))
                return;

            speech.SpeechSounds = component.OriginalSpeechSounds;

            if (component.OriginalSpeechVerb is {} originalSpeechVerb)
                speech.SpeechVerb = originalSpeechVerb;

            if (component.OriginalAllowedEmotes is {} originalAllowedEmotes)
                speech.AllowedEmotes = originalAllowedEmotes;
        }
        // Shitmed Change End
    }
}
