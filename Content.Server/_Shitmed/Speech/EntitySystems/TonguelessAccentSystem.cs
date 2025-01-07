using System.Text;
using System.Text.RegularExpressions;
using Content.Server._Shitmed.Speech.Components;
using Content.Server.Speech;
using Robust.Shared.Random;

namespace Content.Server._Shitmed.Speech.EntitySystems;

public sealed partial class TonguelessAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    [GeneratedRegex("[gdntke]{1,3}")]
    private static partial Regex LowerConsonantRegex();
    private static readonly string[] LowerConsonantReplacements = { "a", "aa", "e", "ee", "o", "oo", "'", "'", "'" };

    [GeneratedRegex("[GDNTKE]{1,3}")]
    private static partial Regex UpperConsonantRegex();
    private static readonly string[] UpperConsonantReplacements = { "A", "AA", "E", "EE", "O", "OO", "'", "'" , "'" };

    [GeneratedRegex("[l]{1,3}")]
    private static partial Regex LowerLRegex();
    private static readonly string[] LowerLReplacements = { "r", "rr", "w", "ww" };

    [GeneratedRegex("[L]{1,3}")]
    private static partial Regex UpperLRegex();
    private static readonly string[] UpperLReplacements = { "R", "RR", "W", "WW" };

    private static readonly Dictionary<char, string> LetterReplacements = new()
    {
        {'q', "k"},
        {'Q', "K"},
        // Can't let tongueless people get away with saying numbers
        {'0', "zero"},
        {'1', "one"},
        {'2', "two"},
        {'3', "three"},
        {'4', "four"},
        {'5', "five"},
        {'6', "six"},
        {'7', "seven"},
        {'8', "eight"},
        {'9', "nine"},
    };

    private static readonly Dictionary<char, char> LetterCReplacements = new() { {'c', 'k'}, {'C', 'K'} };
    private static readonly HashSet<char> LetterCNextCharFilter = new() { 'e', 'h', 'i', 'E', 'H', 'I' };

    [GeneratedRegex(@"(\w)x")]
    private static partial Regex InternalXRegex();
    [GeneratedRegex(@"\bx([\-|r|R]|\b)")]
    private static partial Regex LowerEndXRegex();
    [GeneratedRegex(@"\bX([\-|r|R]|\b)")]
    private static partial Regex UpperEndXRegex();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TonguelessAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, TonguelessAccentComponent component, AccentGetEvent args)
    {
        var message = ReplaceLetters(args.Message);

        message = InternalXRegex().Replace(message, "$1ks");
        message = LowerEndXRegex().Replace(message, "eks$1");
        message = UpperEndXRegex().Replace(message, "EKS$1");

        message = LowerConsonantRegex().Replace(message,
            match => LowerConsonantReplacements[_random.Next(LowerConsonantReplacements.Length)]);
        message = UpperConsonantRegex().Replace(message,
            match => UpperConsonantReplacements[_random.Next(UpperConsonantReplacements.Length)]);

        message = LowerLRegex().Replace(message,
            match => LowerLReplacements[_random.Next(LowerLReplacements.Length)]);
        message = UpperLRegex().Replace(message,
            match => UpperLReplacements[_random.Next(UpperLReplacements.Length)]);

        args.Message = message;
    }

    private string ReplaceLetters(string message)
    {
        var result = new StringBuilder();

        for (int i = 0; i < message.Length; i++)
        {
            var c = message[i];

            if (LetterReplacements.TryGetValue(c, out var replacement))
            {
                result.Append(replacement);
                continue;
            }

            // Replace instances of C with K
            // with a heuristic to determine if they could possibly be pronounced like K
            if (LetterCReplacements.TryGetValue(c, out var letterK) &&
                // Last letter (e.g. basic, fantastic, elastic)
                (i == message.Length - 1 ||
                // Next letter is not E or I (e.g. cent, cell, cistern, civil)
                !LetterCNextCharFilter.Contains(message[i + 1])))
            {
                result.Append(letterK);
                continue;
            }

            result.Append(c);
        }

        return result.ToString();
    }
}
