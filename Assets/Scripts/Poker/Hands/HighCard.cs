using System.Collections.Generic;
using System.Linq;

public class HighCard : PokerHand
{
    public override string example => "KH;JS;5C;4D;2D;";

    public override bool Evaluate(ICollection<ICard> cards) => true;

    public override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        return true;
    }

    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        return cardScripts.OrderByDescending(cs => cs.highCardRank).Take(5).ToArray();
    }
}
