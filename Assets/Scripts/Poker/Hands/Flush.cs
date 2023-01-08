using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PokerHand/Flush")]
public class Flush : PokerHand
{

    [SerializeField]
    private CardComparer flushComparer;
    private IEqualityComparer<ICard> comparer => flushComparer;
    public override bool Evaluate(ICollection<ICard> cards)
    {
        return cards.Where(c => c.suit != null).GroupBy(c => c, comparer).Any(g => g.Count() >= rankingCardsCount);
    }

    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var flushCards = cardScripts
            .Where(c => c.card.suit != null)
            .GroupBy(c => c.card.suit)
            .Where(g => g.Count() >= rankingCardsCount)
            .OrderByDescending(g => g
                .OrderByDescending(c => c.highCardRank)
                .First())
            .First()
            .Take(rankingCardsCount)
            .ToList();
        var kickers = cardScripts
            .Except(flushCards)
            .OrderByDescending(c => c.highCardRank)
            .Take(5 - rankingCardsCount)
            .ToList();

        return flushCards.Concat(kickers).ToArray();
    }
}

