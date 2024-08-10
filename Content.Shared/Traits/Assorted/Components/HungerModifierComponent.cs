using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted.Components;

/// <summary>
///     This is used for traits that modify the base hunger decay rate.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HungerModifierComponent : Component
{
    /// <summary>
    ///     What to multiply the hunger decay rate by.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public float DecayRateMultiplier = 1f;
}
