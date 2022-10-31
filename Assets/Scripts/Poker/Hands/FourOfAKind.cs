using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.PokerHands
{
    public class FourOfAKind : PokerHand
    {
        public override int rank => 70;

        public override int rankingCardsCount => 4;

        public override string example => "AS;AC;AH;AD;KS";

        public override bool Evaluate(ICollection<ICard> cards) => cards.Where(c => c.highCardRank > 0).GroupBy(c => c.highCardRank).Any(g => g.Count() >= 4);

        protected override bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required)
        {
            if (Evaluate(required)) return true;
            var all = cards.Concat(required).ToList();
            var fours = all.Where(c => c.highCardRank > 0).GroupBy(c => c.highCardRank).Where(g => g.Count() >= 4).SelectMany(g => g).ToList();
            if (!fours.Any()) return false;
            return fours.Count(c => required.Contains(c)) >= required.Count - 1;
        }

        protected override CardScript[] GetHand(List<CardScript> cardScripts)
        {
            var groupedByRank = cardScripts.GroupBy(c => c.highCardRank).ToList();
            var cards = groupedByRank.Where(g => g.Count() >= 4).OrderByDescending(g => g.First().highCardRank).First().Take(4).ToList();
            var kicker = cardScripts.Where(c => !cards.Contains(c)).OrderByDescending(cs => cs.highCardRank).First();
            cards.Add(kicker);
            return cards.ToArray();
        }
    }
}
