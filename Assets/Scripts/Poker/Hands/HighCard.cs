using System.Collections.Generic;
using System.Linq;

public class HighCard : PokerHand
{
    public override int rank => 0;

    public override int rankingCardsCount => 1;

    public override string example => "KH;JS;5C;4D;2D;";

    public override bool Evaluate(ICollection<ICard> cards) => true;

    protected override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        return true;
    }

    protected override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        return cardScripts.OrderByDescending(cs => cs.highCardRank).Take(5).ToArray();
    }
}
