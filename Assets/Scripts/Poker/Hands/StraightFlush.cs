using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StraightFlush : PokerHand
{
    [SerializeField]
    private Straight _straight;
    public override bool Evaluate(ICollection<ICard> cards)
    {
        var groups = cards.GroupBy(c => c.suit).Where(g => g.Count() >= 5).ToList();
        if (!groups.Any()) return false;
        foreach (var group in groups)
        {
            var straight = _straight.Evaluate(group.ToList());
            if (straight) return true;
        }
        return false;
    }
    public override string example => "10H;9H;8H;7H;6H";

    public override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        var suit = required.SingleOrDefault(r => r.suit).suit;
        if (suit == null) return false;
        return _straight.EvaluateRequired(cards.Where(c => c.suit == suit).ToList(), required);
    }

    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var groups = cardScripts.GroupBy(c => c.card.suit).Where(g => g.Count() >= 5).OrderByDescending(g => g.First().highCardRank).ToList();
        foreach (var group in groups)
        {
            var hasStraight = _straight.Evaluate(group.Cast<ICard>().ToList());
            if(hasStraight)
            {
                return _straight.GetHand(group.ToList());
            }
        }
        throw new ArgumentException("no straight flush");
    }
}

