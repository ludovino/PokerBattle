using System.Collections.Generic;
using System.Linq;

public class Pair : PokerHand
{
    public override int rank => 10;

    public override int rankingCardsCount => 2;

    public override string example => "AS;AC;4H;3H;2D";

    public bool HasPair(IEnumerable<ICard> cards) => cards
        .Where(c => c != null)
        .Where(c => c.highCardRank > 0)
        .GroupBy(c => c.highCardRank)
        .Any(g => g.Count() > 1);
    
    public override bool Evaluate(ICollection<ICard> cards) => HasPair(cards);

    protected override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        if(Evaluate(required)) return true;
        if (required.Count == 4) return cards.Any(c => required.Contains(c));
        return Evaluate(cards.Concat(required).ToList());
    }

    protected override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var cards = cardScripts.Where(c => c.highCardRank > 0).GroupBy(c => c.highCardRank).OrderByDescending(g => g.Key).First(g => g.Count() >= 2).Take(2).ToList(); // get highest pair
        var kickers = cardScripts.Where(c => !cards.Contains(c)).OrderByDescending(cs => cs.highCardRank).Take(3).ToList(); // get highest kickers
        cards.AddRange(kickers);
        return cards.ToArray();
    }
}
