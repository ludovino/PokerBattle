using System.Collections.Generic;
using UnityEngine;

public abstract class CardComparer : ScriptableObject, IEqualityComparer<ICard>
{
    public abstract bool Equals(ICard x, ICard y);

    public abstract int GetHashCode(ICard obj);
}

