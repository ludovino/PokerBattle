using System.Collections.Generic;
using System.Linq;

public class TwoPair : PokerHand
{
    public override int rank => 20;

    public override int rankingCardsCount => 4;

    public override string example => "AS;AC;KH;KD;QC";

    public override bool Evaluate(ICollection<ICard> cards) => cards
        .Where(c => c.highCardRank > 0)
        .GroupBy(c => c.highCardRank)
        .Where(g => g.Count() >= 2)
        .Count() >= 2;

    protected override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        var all = cards.Concat(required).ToList();
        var pairCards = all
            .Where(c => c.highCardRank > 0)
            .GroupBy(c => c.highCardRank)
            .Where(g => g.Count() >= 2)
            .SelectMany(g => g).ToList();
        if (pairCards.Count < 4) return false;
        return pairCards.Count(c => required.Contains(c)) >= required.Count() - 1;
    }

    protected override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var cards = cardScripts.GroupBy(c => c.highCardRank).Where(g => g.Count() > 1).OrderByDescending(g => g.Key).Take(2).SelectMany(g => g.Take(2)).ToList(); // get top 2 pairs
        var kicker = cardScripts.Where(c => !cards.Contains(c)).OrderByDescending(c => c.highCardRank).FirstOrDefault();
        if (kicker != null) cards.Add(kicker);
        return cards.ToArray();
    }
}
