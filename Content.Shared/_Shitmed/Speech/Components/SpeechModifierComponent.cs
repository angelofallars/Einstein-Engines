using Content.Shared.Chat.Prototypes;
using Content.Shared.Speech;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Shitmed.Speech.Components;

/// <summary>
///     Component to modify SpeechComponent.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SpeechModifierComponent : Component
{
    [DataField, AutoNetworkedField]
    public ProtoId<SpeechSoundsPrototype>? SpeechSounds = null;

    [DataField, AutoNetworkedField]
    public ProtoId<SpeechSoundsPrototype>? OriginalSpeechSounds = null;

    /// <summary>
    ///     What speech verb prototype should be used by default for displaying this entity's messages?
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<SpeechVerbPrototype>? SpeechVerb = null;

    [DataField, AutoNetworkedField]
    public ProtoId<SpeechVerbPrototype>? OriginalSpeechVerb = null;

    /// <summary>
    ///     What emotes allowed to use event if emote <see cref="EmotePrototype.Available"/> is false
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<ProtoId<EmotePrototype>> AllowedEmotes = new();

    [DataField, AutoNetworkedField]
    public List<ProtoId<EmotePrototype>>? OriginalAllowedEmotes = null;
}
