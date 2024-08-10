using Content.Shared.Mood;

namespace Content.Server.Traits.Assorted;

/// <summary>
///     Used for traits that modify how strong mood changes are in the Mood system.
/// </summary>
[RegisterComponent]
public sealed partial class MoodChangeModifierComponent : Component
{
    /// <summary>
    ///   What to multiply mood changes by.
    /// </summary>
    [DataField(required: true)]
    public float Multiplier = 1f;
}
