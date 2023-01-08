using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PokerHand/CardComparer/Paired")]
public class Paired : CardComparer
{
    public override bool Equals(ICard x, ICard y)
    {
        if (x == null || y == null) return false;
        return x.highCardRank == y.highCardRank;
    }

    public override int GetHashCode(ICard obj)
    {
        var card = obj;
        return HashCode.Combine(card.highCardRank);
    }
}

