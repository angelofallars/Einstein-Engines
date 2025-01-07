using Content.Shared.Chat.Prototypes;
using Content.Shared.Humanoid;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.Speech.Components;

/// <summary>
///     Component to modify VocalComponent.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class VocalModifierComponent : Component
{
    [DataField(customTypeSerializer: typeof(PrototypeIdValueDictionarySerializer<Sex, EmoteSoundsPrototype>)), AutoNetworkedField]
    public Dictionary<Sex, string>? Sounds;

    [DataField(customTypeSerializer: typeof(PrototypeIdValueDictionarySerializer<Sex, EmoteSoundsPrototype>)), AutoNetworkedField]
    public Dictionary<Sex, string>? OriginalSounds;

    [DataField, AutoNetworkedField]
    public SoundSpecifier? Wilhelm;

    [DataField, AutoNetworkedField]
    public SoundSpecifier? OriginalWilhelm;

    [DataField, AutoNetworkedField]
    public float? WilhelmProbability;

    [DataField, AutoNetworkedField]
    public float? OriginalWilhelmProbability;
}
