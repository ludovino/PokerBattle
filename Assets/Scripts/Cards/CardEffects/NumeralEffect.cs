using UnityEngine;

public abstract class NumeralEffect : CardEffect
{
    [SerializeField]
    private int _value;
    [SerializeField]
    private Face _face;
    public override bool Condition(ICard card)
    {
        if (card.face == null && _face == null) return card.highCardRank == _value;
        return card.face == _face;
    }
}