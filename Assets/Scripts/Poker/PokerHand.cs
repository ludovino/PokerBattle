using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PokerHand : ScriptableObject
{
    [SerializeField]
    private string _displayName;
    public string DisplayName => _displayName;
    public abstract int rank { get; }
    public abstract int rankingCardsCount { get; }
    public abstract string example { get; }
    public abstract bool Evaluate(ICollection<ICard> cards);
    public bool Evaluate(ICollection<ICard> cards, ICollection<ICard> required)
    {
        if (required.Count >= 5) return Evaluate(required);
        if (required.Count == 0) return Evaluate(cards);
        return EvaluateRequired(cards, required);
    }

    protected abstract bool EvaluateRequired(ICollection<ICard> cards, ICollection<ICard> required);

    public RankedHand GetRankedHand(List<CardScript> cardScripts)
    {
        return new RankedHand()
        {
            cards = GetHand(cardScripts),
            hand = this
        };
    }

    protected abstract CardScript[] GetHand(List<CardScript> cardScripts);
}

public static class PokerHandExtensions
{
    public static bool Suited(this IEnumerable<ICard> cards) => cards.Where(c => c.suit != null).GroupBy(c => c.suit).Count() == 1;
    public static bool Matching(this IEnumerable<ICard> cards) => cards.Where(c => c.highCardRank > 0).GroupBy(c => c.highCardRank).Count() == 1;
    public static bool Sequential(this IEnumerable<ICard> cards) => Straight.HasStraight(cards.ToList(), cards.Count());
}