public interface ICard
{
    int blackjackValue { get; }
    int highCardRank { get; }
    string numeral { get; }
    Suit suit { get; }
    Face face { get; }

    string ToString();
}