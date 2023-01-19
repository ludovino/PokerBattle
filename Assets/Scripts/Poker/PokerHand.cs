using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PokerHand : ScriptableObject
{
    [SerializeField]
    private string _displayName;

    [SerializeField]
    private int _rank;

    [SerializeField]
    private int _rankingCardsCount;
    [SerializeField]
    private string _example;
    public string DisplayName => _displayName;
    public int rank => _rank;
    public int rankingCardsCount => _rankingCardsCount;
    public string example => _example;
    [SerializeField]
    private int _score;
    public int score => _score;

    public abstract bool Evaluate(ICollection<ICard> cards);

    public RankedHand GetRankedHand(List<CardScript> cardScripts)
    {
        return new RankedHand()
        {
            cards = GetHand(cardScripts),
            hand = this
        };
    }

    public abstract CardScript[] GetHand(List<CardScript> cardScripts);
}

public static class PokerHandExtensions
{
    public static bool Suited(this IEnumerable<ICard> cards) => cards.Where(c => c.suit != null).GroupBy(c => c.suit).Count() == 1;
    public static bool Matching(this IEnumerable<ICard> cards) => cards.Where(c => c.highCardRank > 0).GroupBy(c => c.highCardRank).Count() == 1;
}