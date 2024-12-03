using Content.Shared.Chat.Prototypes;
using Content.Shared.Chat.TypingIndicator;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Speech;

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

    /// <summary>
    ///     Prototype ID that stores all visual info about typing indicator.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<TypingIndicatorPrototype>)), AutoNetworkedField]
    public string? TypingIndicator = null;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<TypingIndicatorPrototype>)), AutoNetworkedField]
    public string? OriginalTypingIndicator = null;
}
