using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted.Components;

/// <summary>
///     This is used for traits that modify the base thirst decay rate.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ThirstModifierComponent : Component
{
    /// <summary>
    ///     What to multiply the thirst decay rate by.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public float DecayRateMultiplier = 1f;
}
