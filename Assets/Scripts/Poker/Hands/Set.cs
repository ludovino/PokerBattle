
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PokerHand/Set")]
internal class Set : PokerHand
{
    [SerializeField]
    private CardComparer setComparer;
    private IEqualityComparer<ICard> comparer => setComparer;
    public override bool Evaluate(ICollection<ICard> cards) => cards.Where(c => c.highCardRank > 0).GroupBy(c => c, comparer).Any(g => g.Count() >= rankingCardsCount);

    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var cards = cardScripts.GroupBy(c => c, comparer).Where(g => g.Count() >= rankingCardsCount).OrderByDescending(g => g.Key).First().Take(rankingCardsCount).ToList();
        var kickers = cardScripts.Where(c => !cards.Contains(c)).OrderByDescending(c => c.highCardRank).Take(5 - rankingCardsCount).ToList();
        cards.AddRange(kickers);
        return cards.ToArray();
    }
}

