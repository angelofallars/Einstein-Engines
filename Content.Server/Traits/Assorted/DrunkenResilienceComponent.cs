using Content.Shared.Damage;
using Content.Shared.FixedPoint;

namespace Content.Server.Traits.Assorted;

/// <summary>
///   This is used for the Drunken Resilience trait.
/// </summary>
[RegisterComponent]
public sealed partial class DrunkenResilienceComponent : Component
{
    /// <summary>
    /// The next time that drunkenness will be updated and healing done.
    /// </summary>
    [DataField]
    public TimeSpan NextUpdate;

    /// <summary>
    /// The interval at which this component updates.
    /// </summary>
    [DataField]
    public TimeSpan UpdateInterval = TimeSpan.FromSeconds(1);

    /// <summary>
    ///   How drunk is this entity?
    /// </summary>
    [DataField]
    public FixedPoint2 Drunkenness = 0f;

    /// <summary>
    ///   The minimum threshold of MaxDrunkenness in which to start drunk healing.
    /// </summary>
    [DataField]
    public FixedPoint2 MinHealingThreshold = 5f;

    /// <summary>
    ///   The maximum amount of drunkenness.
    /// </summary>
    [DataField]
    public FixedPoint2 MaxDrunkenness = 100f;

    /// <summary>
    ///   Damage to apply every interval. Scaled by the ratio of drunkenness to the max drunk amount.
    /// </summary>
    [DataField(required: true)]
    public DamageSpecifier Damage = default!;

    /// <summary>
    ///   How much Drunkenness to reduce every interval.
    /// </summary>
    [DataField]
    public FixedPoint2 DrunkennessReductionAmount = 0.30f;

    /// <summary>
    ///   What to multiply drunkenness increases by (usually 2).
    /// </summary>
    [DataField]
    public FixedPoint2 DrunkennessIncreaseModifier = 0.25f;
}
