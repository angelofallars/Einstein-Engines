using Content.Shared._Shitmed.Speech.EntitySystems; // Shitmed Change
using Robust.Shared.GameStates; // Shitmed Change
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Chat.TypingIndicator;

/// <summary>
///     Show typing indicator icon when player typing text in chat box.
///     Added automatically when player poses entity.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState] // Shitmed Change - make field autonetworked
[Access(typeof(SharedTypingIndicatorSystem), typeof(SpeechModifierSystem))] // Shitmed Change - give access to SpeechModifierSystem
public sealed partial class TypingIndicatorComponent : Component
{
    /// <summary>
    ///     Prototype id that store all visual info about typing indicator.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("proto", customTypeSerializer: typeof(PrototypeIdSerializer<TypingIndicatorPrototype>)), AutoNetworkedField] // Shitmed Change - make field autonetworked
    public string Prototype = SharedTypingIndicatorSystem.InitialIndicatorId;
}
