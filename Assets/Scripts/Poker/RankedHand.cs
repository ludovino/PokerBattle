using System;
using System.Linq;

public class RankedHand : IComparable<RankedHand>
{
    public CardScript[] cards;
    public PokerHand hand;
    public int rank => hand.rank;
    public int CompareTo(RankedHand other)
    {
        if(this.hand.rank > other.hand.rank) return 1;
        if(this.hand.rank < other.hand.rank) return -1;
        for (int i = 0; i < 5; i++)
        {
            if (this.cards[i].highCardRank > other.cards[i].highCardRank) return 1;
            if (this.cards[i].highCardRank < other.cards[i].highCardRank) return -1;
        }
        return 0;
    }

    public static bool operator > (RankedHand operand1, RankedHand operand2) => operand1.CompareTo(operand2) > 0;
    public static bool operator < (RankedHand operand1, RankedHand operand2) => operand1.CompareTo(operand2) < 0;
    public static bool operator >= (RankedHand operand1, RankedHand operand2) => operand1.CompareTo(operand2) >= 0;
    public static bool operator <= (RankedHand operand1, RankedHand operand2) => operand1.CompareTo(operand2) <= 0;
    public int chipCost => cards.Take(hand.rankingCardsCount).Sum(c => c.blackjackValue);
    public override string ToString()
    {
        var cardsString = string.Join(", ", cards.Select(c => c.card.ToString()));
        return $"{hand.name} {cardsString}";
    }
}