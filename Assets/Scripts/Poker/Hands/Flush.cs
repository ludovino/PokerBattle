using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Flush : PokerHand
{
    public override int rank => 50;

    public override int rankingCardsCount => 4;

    public override string example => "AS;JS;10S;8S;7S";

    public override bool Evaluate(ICollection<ICard> cards)
    {
        return cards.Where(c => c.suit != null).GroupBy(c => c.suit.shortName).Any(g => g.Count() >= 5);
    }

    protected override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        var suit = required.SingleOrDefault(r => r.suit).suit;
        if(suit == null) return false;
        return cards.Concat(required).Count(c => c.suit == suit) >= 5;
    }

    protected override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        return cardScripts
            .Where(c => c.card.suit != null)
            .GroupBy(c => c.card.suit)
            .Where(g => g.Count() >= 5)
            .OrderByDescending(g => g
                .OrderByDescending(c => c.highCardRank)
                .First())
            .First()
            .Take(5).ToArray();
    }
}

