using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ThreeOfAKind : PokerHand
{
    public override int rank => 30;

    public override int rankingCardsCount => 3;

    public override string example => "KH;KC;KS;9H;8D";

    public override bool Evaluate(ICollection<ICard> cards)
    {
        return cards.Where(c => c.highCardRank > 0).GroupBy(c => c.highCardRank).Any(g => g.Count() >= 3);
    }

    protected override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
    {
        if (Evaluate(required)) return true;
        var all = cards.Concat(required).ToList();
        var threes = all
            .Where(c => c.highCardRank > 0)
            .GroupBy(c => c.highCardRank)
            .Where(g => g.Count() >= 3)
            .SelectMany(g => g).ToList();
        if (!threes.Any()) return false;
        return threes.Count(c => required.Contains(c)) >= required.Count - 2;
    }

    protected override CardScript[] GetHand(List<CardScript> cardScripts)
    {
        var cards = cardScripts.GroupBy(c => c.highCardRank).Where(g => g.Count() >= 3).OrderByDescending(g => g.Key).First().Take(3).ToList();
        var kickers = cardScripts.Where(c => !cards.Contains(c)).OrderByDescending(c => c.highCardRank).Take(2).ToList();
        cards.AddRange(kickers);
        return cards.ToArray();
    }
}

