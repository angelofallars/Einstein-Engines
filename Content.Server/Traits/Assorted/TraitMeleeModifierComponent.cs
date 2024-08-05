using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.Traits.Assorted;

/// <summary>
/// This is used for traits that modify unarmed melee attacks using the MeleeWeaponSystem.
/// Unless specified, the fields replace their equivalent fields in the original component.
/// </summary>
[RegisterComponent]
public sealed partial class TraitMeleeModifierComponent : Component
{
    [DataField]
    public DamageSpecifier? Damage = default!;

    [DataField]
    public SoundSpecifier? SoundHit;

    [DataField]
    public EntProtoId Animation = "";

    [DataField]
    public EntProtoId WideAnimation = "";

    [DataField]
    public float? AttackRate;
}
