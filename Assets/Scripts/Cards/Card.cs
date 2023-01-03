
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
    public int price => (_face?.price ?? _value) + (suit?.price ?? 0);
    private string GetNumeral()
    {
        var numeral = _face?.numeral ?? _value.ToString();
        if (_value == 0) numeral = string.Empty;
        return numeral;
    }
    public Suit suit => _suit;

    public Face face => _face;

    public void Change(int change)
    {
        if (change == 0) return;
        if(_face is null)
        {
            _value = Mathf.Clamp(_value + change, 0, 21);
            return;
        }
        if (change > 0 && _face.higher != null)
        {
            _face = _face.higher;
            return;
        }
        if(change < 0)
        {
            _face = _face.lower;
        }
        if (_face is null)
        {
            _value = 10;
        }
    }

    public void SetSuit(Suit suit)
    {
        _suit = suit;
    }

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
        return ToString() == (other?.ToString() ?? null);
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
