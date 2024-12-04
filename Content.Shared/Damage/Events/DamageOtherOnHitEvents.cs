using Content.Shared.Damage;
using Content.Shared.Item.ItemToggle.Components;

namespace Content.Shared.Damage.Events;

/// <summary>
///   Raised on a throwing weapon to calculate potential damage bonuses or decreases.
/// </summary>
[ByRefEvent]
public record struct GetThrowingDamageEvent(EntityUid Weapon, DamageSpecifier Damage, List<DamageModifierSet> Modifiers, EntityUid? User);

/// <summary>
///   Raised on a throwing weapon when ItemToggleDamageOtherOnHit has been successfully initialized.
/// </summary>
public record struct ItemToggleDamageOtherOnHitStartup(Entity<ItemToggleDamageOtherOnHitComponent> Weapon);
