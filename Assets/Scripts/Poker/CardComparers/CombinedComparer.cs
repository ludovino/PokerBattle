using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PokerHand/CardComparer/Combined")]
internal class CombinedComparer : CardComparer
{
    public List<CardComparer> comparers;
    public override bool Equals(ICard x, ICard y)
    {
        if (comparers == null)
        {
            return false;
        }

        return comparers.All(c => c.Equals(x, y));
    }

    public override int GetHashCode(ICard obj)
    {
        return comparers.Aggregate(0, (int hash, CardComparer comparer) => HashCode.Combine(comparer, hash));
    }
}
