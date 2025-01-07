using Content.Shared.Chat.TypingIndicator;
using Content.Shared.Speech;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._Shitmed.Speech.Components;

/// <summary>
///     Component to modify SpeechComponent.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SpeechModifierComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public ProtoId<SpeechSoundsPrototype>? SpeechSounds = null;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public ProtoId<SpeechSoundsPrototype>? OriginalSpeechSounds = null;

    /// <summary>
    ///     What speech verb prototype should be used by default for displaying this entity's messages?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public ProtoId<SpeechVerbPrototype>? SpeechVerb = null;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public ProtoId<SpeechVerbPrototype>? OriginalSpeechVerb = null;
}
