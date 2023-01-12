using UnityEngine;

public abstract class SuitEffect : CardEffect, ICardEffect
{
    [SerializeField]
    private Suit _suit;
    public Suit Suit => _suit;

    public override bool Condition(ICard card)
    {
        return card.suit == Suit;
    }
}