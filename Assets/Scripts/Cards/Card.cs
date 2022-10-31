
using System;
using UnityEngine;
[System.Serializable]
public class Card : ICard, IEquatable<Card>
{
    public Card() { }
    public Card(Suit suit, int value = 0, Face face = null)
    {
        _suit = suit;
        _value = value;
        _face = face;
    }
    [SerializeField]
    private int _value;
    [SerializeField]
    private Face _face;
    [SerializeField]
    private Suit _suit;

    public string numeral => GetNumeral();
    public int highCardRank => _face?.highCardRank ?? _value;
    public int blackjackValue => _face?.blackjackValue ?? Mathf.Max(_value, 1);
    private string GetNumeral()
    {
        var numeral = _face?.numeral ?? _value.ToString();
        if (_value == 0) numeral = string.Empty;
        return numeral;
    }
    public Suit suit => _suit;

    public Face face => _face;

    public override string ToString()
    {
        var numeralName = _face?.numeral ?? _value.ToString();
        var suitName = _suit?.shortName ?? "-";
        return $"{numeralName}{suitName}";
    }

    public Card Clone()
    {
        return new Card(_suit, _value, _face);
    }
    public bool Equals(Card other)
    {
        return this.ToString() == other.ToString();
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(this.ToString());
    }
    public override bool Equals(object obj) => Equals(obj as Card);
    public static bool operator ==(Card lhs, Card rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }

            // Only the left side is null.
            return false;
        }
        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Card lhs, Card rhs) => !(lhs == rhs);
}
