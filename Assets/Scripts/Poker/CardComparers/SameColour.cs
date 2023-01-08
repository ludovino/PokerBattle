using System;

namespace Assets.Scripts.Poker.CardComparers
{
    internal class SameColour : CardComparer
    {
        public override bool Equals(ICard x, ICard y)
        {
            if (x == null || y == null) return false;
            return x.suit.Color == y.suit.Color;
        }

        public override int GetHashCode(ICard obj)
        {
            return HashCode.Combine(obj.suit != null ? obj.suit.Color : null);
        }
    }
}
