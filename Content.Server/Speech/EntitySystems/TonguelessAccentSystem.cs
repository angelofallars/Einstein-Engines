using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class TonguelessAccentSystem : EntitySystem
{
    // Yes, this is unashamedly taken from /tg/ station.
    private static readonly Regex RegexTonguelessLower = new Regex(@"[gdntke]+");
    private static readonly Regex RegexTonguelessUpper = new Regex(@"[GDNTKE]+");
    private static readonly List<String> ReplacementsLower = new List<String> { "aa", "oo", "'" };
    private static readonly List<String> ReplacementsUpper = new List<String> { "AA", "OO", "'" };
    private static readonly Random Random = new Random();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TonguelessAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, TonguelessAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        // Replace lower-case matches
        message = RegexTonguelessLower.Replace(message, match =>
            ReplacementsLower[Random.Next(ReplacementsLower.Count)]);

        // Replace upper-case matches
        message = RegexTonguelessUpper.Replace(message, match =>
            ReplacementsUpper[Random.Next(ReplacementsUpper.Count)]);

        args.Message = message;
    }
}
