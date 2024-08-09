using Content.Shared.Damage;
using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted.Components;

/// <summary>
/// This is used for the Way Of The Open Hand trait
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class MonkComponent : Component
{
    [DataField(required: true), AutoNetworkedField]
    public DamageModifierSet MeleeModifiers = default!;

    [DataField(required: true), AutoNetworkedField]
    public DamageSpecifier MeleeFlatBonuses = default!;

    [DataField, AutoNetworkedField]
    public float MeleeRangeModifier = 1.5f;

    [DataField, AutoNetworkedField]
    public float AttackRateModifier = 2;

    /// <summary>
    ///     When attempting attack against the same entity multiple times,
    ///     don't spam popups every frame and instead have a cooldown.
    /// </summary>
    [DataField]
    public TimeSpan PopupCooldown = TimeSpan.FromSeconds(3.0);

    [DataField]
    [AutoPausedField]
    public TimeSpan? NextPopupTime = null;

    /// <summary>
    ///     The last entity attacked, used for popup purposes (avoid spam)
    /// </summary>
    [DataField]
    public EntityUid? LastAttackedEntity = null;
}
