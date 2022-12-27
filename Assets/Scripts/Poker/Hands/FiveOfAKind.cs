using System.Collections.Generic;
using System.Linq;

public class FiveOfAKind : PokerHand
{
    public override string example => "AS;AC;AH;AD;AD";

    public override bool Evaluate(ICollection<ICard> cards)
    {
        return cards.Where(c => c.highCardRank > 0).GroupBy(c => c.highCardRank).Any(g => g.Count() >= 5);
    }

    public override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        var rank = required.Where(c => c.highCardRank > 0).Select(r => r.highCardRank).SingleOrDefault();
        if (rank == 0) return false;
        return cards.Concat(required).Count(c => c.highCardRank == rank) >= 5;
    }

    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        return cardScripts
            .GroupBy(c => c.highCardRank)
            .Where(g => g.Count() >= 5)
            .OrderByDescending(g => g.First().highCardRank)
            .First()
            .Take(5)
            .ToArray();
    }
}

