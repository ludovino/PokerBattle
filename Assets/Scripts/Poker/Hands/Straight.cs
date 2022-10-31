using System.Collections.Generic;
using System.Linq;

public class Straight : PokerHand
{
    public override int rank => 40;

    public override int rankingCardsCount => 5;

    public override string example => "19H;20C;21D;JS;QH";

    private readonly static IReadOnlyList<string> oneLow = new List<string> { "1", "2", "3", "4", "5" };
    private readonly static IReadOnlyList<string> aceLow = new List<string> { "A", "2", "3", "4", "5" };
    private readonly static IReadOnlyList<string> numbers = Enumerable.Range(2, 20).Select(i => i.ToString()).ToList();
    private readonly static IReadOnlyList<string> aceHigh = new List<string> { "7", "8", "9", "10", "J", "Q", "K", "A" };
    private readonly static IReadOnlyList<string> aceVeryHigh = new List<string> { "18", "19", "20", "21", "J", "Q", "K", "A" };
    private readonly static IReadOnlyList<IReadOnlyList<string>> straightSequences = new List<IReadOnlyList<string>> { oneLow, aceLow, numbers, aceHigh, aceVeryHigh };
    public override bool Evaluate(ICollection<ICard> cards)
    {
        return HasStraight(cards);
    }

    private static HashSet<string> GetNumerals(IEnumerable<ICard> cards)
    {
        return cards.Select(c => c.numeral).Distinct().ToHashSet();
    }

    private static bool HasStraight(HashSet<string> numerals, IReadOnlyList<string> sequence, int length)
    {
        var count = 0;
        for (int i = 0; i < sequence.Count; i++)
        {
            count++;
            if (!numerals.Contains(sequence[i])) count = 0;
            if (count == length) return true;
        }
        return false;
    }

    public static bool HasStraight(ICollection<ICard> cards, int length = 5)
    {
        if (cards.Count < length) return false;
        var numerals = GetNumerals(cards);
        return straightSequences.Any(s => HasStraight(numerals, s, length));
    }

    private static List<string> HighestStraight(HashSet<string> numerals, IReadOnlyList<string> sequence)
    {
        var result = new List<string>();
        for (int i = sequence.Count() - 1; i >= 0; i--)
        {
            var current = sequence[i];
            if (numerals.Contains(current)) result.Add(current);
            else result.Clear();
            if (result.Count == 5) return result;
        }
        return null;
    }
    protected static List<List<string>> GetStraightSequences(HashSet<string> numerals)
    {
        var results = new List<List<string>>();

        foreach (var sequence in straightSequences)
        {
            var result = new List<string>();
            for (int i = sequence.Count() - 1; i >= 0; i--)
            {
                var current = sequence[i];
                if (numerals.Contains(current)) result.Add(current);
                else if (result.Count >= 5)
                {
                    results.Add(result);
                    result = new List<string>();
                }
            }
        }
        return results;
    }

    public static List<Card> GetStraightCards(List<Card> cards)
    {
        var numerals = GetNumerals(cards);
        var straightNumerals = GetStraightSequences(numerals).SelectMany(l => l).Distinct().ToList();

        return cards.Where(c => straightNumerals.Contains(c.numeral)).ToList();
    }

    public static NextInSequence NextSequentialValues(List<Card> cards)
    {
        if(cards.Count == 0) return null;
        if (!cards.Sequential()) return null;
        var numerals = GetNumerals(cards);
        var result = new NextInSequence();
        foreach (var sequence in straightSequences)
        {
            for (int i = sequence.Count - 2; i >= 0; i--)
            {
                var current = sequence[i];
                var previous = sequence[i + 1];
                if (numerals.Contains(current) && !numerals.Contains(previous)) 
                {
                    result.after.Add(previous);
                    continue;
                }
                if (!numerals.Contains(current) && numerals.Contains(previous))
                {
                    result.before.Add(current);
                    continue;
                }
            }
        }

        return result;
    }
    public class NextInSequence
    {
        public NextInSequence()
        {
            before = new List<string>();
            after = new List<string>();
        }
        public List<string> before;
        public List<string> after;

        public bool Contains(Card card)
        {
            return before.Contains(card.numeral) || after.Contains(card.numeral);
        }
    }
    protected override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var numerals = GetNumerals(cardScripts);
        var straightNumerals =
            HighestStraight(numerals, aceVeryHigh) ??
            HighestStraight(numerals, aceHigh) ??
            HighestStraight(numerals, numbers) ??
            HighestStraight(numerals, aceLow) ??
            HighestStraight(numerals, oneLow);
        return cardScripts.GroupBy(c => c.card.numeral).Select(g => g.First()).Where(c => straightNumerals.Contains(c.card.numeral)).Take(5).ToArray();
    }

    protected override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        var requiredNumerals = required.Select(c => c.numeral).Distinct().ToList();
        if (requiredNumerals.Count < required.Count) return false;
        var allNumerals = GetNumerals(cards.Concat(required).ToList());
        var sequences = GetStraightSequences(allNumerals);
        if (sequences.Count == 0) return false;
        foreach (var sequence in sequences)
        {
            var indicies = requiredNumerals.Select(r => sequence.IndexOf(r));
            var highest = indicies.Max();
            var lowest = indicies.Min();
            if (highest - lowest <= 5 && lowest >= 0) return true;
        }
        return false;
    }
}

