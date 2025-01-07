using Content.Shared._Shitmed.Chat.TypnigIndicator; // Shitmed Change
using Content.Shared.Clothing;

namespace Content.Shared.Chat.TypingIndicator;

/// <summary>
///     Sync typing indicator icon between client and server.
/// </summary>
public abstract class SharedTypingIndicatorSystem : EntitySystem
{
    /// <summary>
    ///     Default ID of <see cref="TypingIndicatorPrototype"/>
    /// </summary>
    [ValidatePrototypeId<TypingIndicatorPrototype>]
    public const string InitialIndicatorId = "default";

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TypingIndicatorClothingComponent, ClothingGotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<TypingIndicatorClothingComponent, ClothingGotUnequippedEvent>(OnGotUnequipped);

        // Shitmed Change Start
        SubscribeLocalEvent<TypingIndicatorModifierComponent, ComponentStartup>(OnModifierStartup);
        SubscribeLocalEvent<TypingIndicatorModifierComponent, ComponentShutdown>(OnModifierShutdown);
        // Shitmed Change End
    }

    // Shitmed Change Start
    private void OnModifierStartup(EntityUid uid, TypingIndicatorModifierComponent component, ComponentStartup args)
    {
        var indicator = EnsureComp<TypingIndicatorComponent>(uid);

        component.OriginalTypingIndicator = indicator.Prototype;

        if (component.TypingIndicator == null)
            return;

        indicator.Prototype = component.TypingIndicator;
        Dirty(uid, indicator);
    }

    private void OnModifierShutdown(EntityUid uid, TypingIndicatorModifierComponent component, ComponentShutdown args)
    {
        if (!TryComp<TypingIndicatorComponent>(uid, out var indicator) ||
            String.IsNullOrEmpty(component.OriginalTypingIndicator))
            return;

        indicator.Prototype = component.OriginalTypingIndicator;
        Dirty(uid, indicator);
    }
    // Shitmed Change End

    private void OnGotEquipped(EntityUid uid, TypingIndicatorClothingComponent component, ClothingGotEquippedEvent args)
    {
        if (!TryComp<TypingIndicatorComponent>(args.Wearer, out var indicator))
            return;

        indicator.Prototype = component.Prototype;
    }

    private void OnGotUnequipped(EntityUid uid, TypingIndicatorClothingComponent component, ClothingGotUnequippedEvent args)
    {
        if (!TryComp<TypingIndicatorComponent>(args.Wearer, out var indicator))
            return;

        indicator.Prototype = SharedTypingIndicatorSystem.InitialIndicatorId;
    }
}
