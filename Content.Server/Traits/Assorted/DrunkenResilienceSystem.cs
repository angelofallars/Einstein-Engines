using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Timing;

namespace Content.Server.Traits.Assorted;

public sealed class DrunkenResilienceSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly DamageableSystem _damageableSys = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DrunkenResilienceComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<DrunkenResilienceComponent, EntityUnpausedEvent>(OnUnpaused);
    }

    private void OnMapInit(Entity<DrunkenResilienceComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextUpdate = _gameTiming.CurTime + ent.Comp.UpdateInterval;
    }

    private void OnUnpaused(Entity<DrunkenResilienceComponent> ent, ref EntityUnpausedEvent args)
    {
        ent.Comp.NextUpdate += args.PausedTime;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DrunkenResilienceComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (_gameTiming.CurTime < comp.NextUpdate)
                continue;

            comp.NextUpdate += comp.UpdateInterval;

            if !_mobStateSystem.IsDead(uid))
                continue;

            // Heal based on their Drunkenness
            if (comp.Drunkenness >= comp.MinHealingThreshold)
            {
                var percentage = comp.Drunkenness / comp.MaxDrunkenness;

                var damage = comp.Damage * percentage;

                _damageableSys.TryChangeDamage(uid, damage, ignoreResistances: true, interruptsDoAfters: false);
            }

            // Reduce drunkenness
            comp.Drunkenness = FixedPoint2.Max(FixedPoint2.Zero, comp.Drunkenness - comp.DrunkennessReductionAmount);
        }
    }

    public void AddDrunkenness(EntityUid uid, float boozePower, DrunkenResilienceComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        var increase = FixedPoint2.New(boozePower) * comp.DrunkennessIncreaseModifier;

        comp.Drunkenness = FixedPoint2.Min(comp.MaxDrunkenness, comp.Drunkenness + increase);
    }
}
