using System.Collections.Generic;
using System.Linq;

internal class Rainbow : PokerHand
{
    public override bool Evaluate(ICollection<ICard> cards)
    {
        return cards.Where(c => c.suit != null).GroupBy(c => c.suit).Count() > rankingCardsCount;
    }

    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var hand = cardScripts.Where(c => c.suit != null).GroupBy(c => c.suit).Select(g => g.First()).Take(rankingCardsCount).ToArray();
        if (rankingCardsCount == 5) return hand;
        var kickers = cardScripts.Except(hand).OrderBy(c => c.highCardRank);
        return hand.Concat(kickers).Take(5).ToArray();
    }
}

