using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Straight : PokerHand
{
    public override string example => "19H;20C;21D;JS;QH";
    
    [SerializeField]
    private string[] sequences;

    private IReadOnlyList<IReadOnlyList<string>> straightSequences;
    public void OnEnable()
    {
        straightSequences = sequences.Select(i => i.Split(';')).ToList();
    }
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

    public bool HasStraight(ICollection<ICard> cards, int length = 5)
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
    protected List<List<string>> GetStraightSequences(HashSet<string> numerals)
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

    public List<Card> GetStraightCards(List<Card> cards)
    {
        var numerals = GetNumerals(cards);
        var straightNumerals = GetStraightSequences(numerals).SelectMany(l => l).Distinct().ToList();

        return cards.Where(c => straightNumerals.Contains(c.numeral)).ToList();
    }
    public NextInSequence NextSequentialValues(List<ICard> cards)
    {
        
        if(cards.Count == 0) return null;
        if (!HasStraight(cards.Cast<ICard>().ToList(), cards.Count)) return null;
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
    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var numerals = GetNumerals(cardScripts);
        var straightNumerals = straightSequences.Select(s => HighestStraight(numerals, s)).First(s => s != null);
        return cardScripts.GroupBy(c => c.card.numeral).Select(g => g.First()).Where(c => straightNumerals.Contains(c.card.numeral)).Take(5).ToArray();
    }

    public override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
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

