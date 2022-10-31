using System;
using System.Collections.Generic;
using System.Linq;

public class StraightFlush : Straight
{
    public override int rank => 80;
    public override bool Evaluate(ICollection<ICard> cards)
    {
        var groups = cards.GroupBy(c => c.suit).Where(g => g.Count() >= 5).ToList();
        if (!groups.Any()) return false;
        foreach (var group in groups)
        {
            var straight = base.Evaluate(group.ToList());
            if (straight) return true;
        }
        return false;
    }
    public override string example => "10H;9H;8H;7H;6H";

    protected override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        var suit = required.SingleOrDefault(r => r.suit).suit;
        if (suit == null) return false;
        return base.EvaluateRequired(cards.Where(c => c.suit == suit).ToList(), required);
    }

    protected override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var groups = cardScripts.GroupBy(c => c.card.suit).Where(g => g.Count() >= 5).OrderByDescending(g => g.First().highCardRank).ToList();
        foreach (var group in groups)
        {
            var hasStraight = base.Evaluate(group.Cast<ICard>().ToList());
            if(hasStraight)
            {
                return base.GetHand(group.ToList());
            }
        }
        throw new ArgumentException("no straight flush");
    }
}

