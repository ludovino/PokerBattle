using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PokerHand/CardComparer/Suited")]
public class Suited : CardComparer
{
    public override bool Equals(ICard x, ICard y)
    {
        if (x == null || y == null) return false;
        return x.suit == y.suit;
    }

    public override int GetHashCode(ICard obj)
    {
        return HashCode.Combine(obj.suit?.shortName ?? "-");
    }
}

