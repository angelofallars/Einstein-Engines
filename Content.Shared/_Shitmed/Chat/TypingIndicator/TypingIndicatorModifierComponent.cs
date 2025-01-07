using Content.Shared.Chat.TypingIndicator;
using Content.Shared.Speech;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._Shitmed.Chat.TypnigIndicator;

/// <summary>
///     Component to modify TypingIndicatorComponent.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TypingIndicatorModifierComponent : Component
{
    /// <summary>
    ///     Prototype ID that stores all visual info about typing indicator.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("proto", customTypeSerializer: typeof(PrototypeIdSerializer<TypingIndicatorPrototype>)), AutoNetworkedField]
    public string? TypingIndicator = null;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<TypingIndicatorPrototype>)), AutoNetworkedField]
    public string? OriginalTypingIndicator = null;
}
