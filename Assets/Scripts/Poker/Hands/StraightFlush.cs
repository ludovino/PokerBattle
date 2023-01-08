using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PokerHand/StraightFlush")]
public class StraightFlush : PokerHand
{
    [SerializeField]
    private Straight _straight;

    [SerializeField]
    private CardComparer flushComparer;
    private IEqualityComparer<ICard> comparer => flushComparer;

    public override bool Evaluate(ICollection<ICard> cards)
    {
        var groups = cards.GroupBy(c => c, comparer).Where(g => g.Count() >= 5).ToList();
        if (!groups.Any()) return false;
        foreach (var group in groups)
        {
            var straight = _straight.Evaluate(group.ToList());
            if (straight) return true;
        }
        return false;
    }

    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var groups = cardScripts.GroupBy(c => c, comparer).Where(g => g.Count() >= 5).OrderByDescending(g => g.First().highCardRank).ToList();
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

