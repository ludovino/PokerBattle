using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PokerHand/TwoSet")]
public class TwoSet : PokerHand
{
    [SerializeField]
    private CardComparer setComparer;
    private IEqualityComparer<ICard> comparer => setComparer;
    private int highGroupSize => rankingCardsCount - 2;
    public override bool Evaluate(ICollection<ICard> cards)
    {
        var groups = cards.Where(c => c.highCardRank > 0).GroupBy(c => c, comparer).ToList();
        return groups.Count(g => g.Count() >= highGroupSize) >= 1 && groups.Count(g => g.Count() >= 2) >= 2;
    }

    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var groups = cardScripts.Where(c => c.highCardRank > 0).GroupBy(c => c, comparer).Where(g => g.Count() >= 2).ToList();
        var group1 = groups.Where(g => g.Count() >= highGroupSize).OrderByDescending(g => g.First().highCardRank).First();
        groups.Remove(group1);
        var group2 = groups.OrderByDescending(g => g.First().highCardRank).First();
        var hand = group1.Take(highGroupSize).ToList();
        hand.AddRange(group2.Take(2));
        var kicker = cardScripts.Except(hand).OrderByDescending(c => c.highCardRank).Take(5-rankingCardsCount).SingleOrDefault();
        if(kicker != null) hand.Add(kicker);
        return hand.ToArray();
    }

    private void OnValidate()
    {
        if (rankingCardsCount < 4) throw new System.Exception("two set hands can't be smaller than 4 cards");
    }
}

