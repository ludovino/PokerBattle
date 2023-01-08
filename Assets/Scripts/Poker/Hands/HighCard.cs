using System.Collections.Generic;
using System.Linq;

public class HighCard : PokerHand
{

    public override bool Evaluate(ICollection<ICard> cards) => true;

    public override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        return cardScripts.OrderByDescending(cs => cs.highCardRank).Take(5).ToArray();
    }
}
