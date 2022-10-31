using System.Collections.Generic;
using System.Linq;

internal class FullHouse : PokerHand
{
    public override int rank => 60;

    public override int rankingCardsCount => 5;

    public override string example => "10H;10C;10D;KD;KS";

    public override bool Evaluate(ICollection<ICard> cards)
    {
        var groups = cards.Where(c => c.highCardRank > 0).GroupBy(c => c.highCardRank).ToList();
        return groups.Count(g => g.Count() >= 3) >= 1 && groups.Count(g => g.Count() >= 2) >= 2;
    }

    protected override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        var distinctValues = required.Select(r => r.highCardRank).Where(v => v > 0).Distinct().ToList();
        if (distinctValues.Count > 2 || required.Count == 4 && distinctValues.Count == 1) return false;
        var all = cards.Concat(required).ToList();
        if (distinctValues.Count == 1) return (Evaluate(all));
        return Evaluate(all.Where(c => distinctValues.Contains(c.highCardRank)).ToList());
       
    }

    protected override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var groups = cardScripts.Where(c => c.highCardRank > 0).GroupBy(c => c.highCardRank).Where(g => g.Count() >= 2).ToList();
        var threeGroup = groups.Where(g => g.Count() >= 3).OrderByDescending(g => g.First().highCardRank).First();
        groups.Remove(threeGroup);
        var twoGroup = groups.OrderByDescending(g => g.First().highCardRank).First();
        var hand = threeGroup.Take(3).ToList();
        hand.AddRange(twoGroup.Take(2));
        return hand.ToArray();
    }
}

