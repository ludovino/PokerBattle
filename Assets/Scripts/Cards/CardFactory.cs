using UnityEngine;
using URandom = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "CardFactory")]
public class CardFactory : ScriptableObject
{

    [SerializeField]
    private List<Suit> _suits;
    [SerializeField]
    private List<Face> _faces;
    [SerializeField]
    private int _highestNumeral;
    void Awake()
    {
        _suits = Resources.LoadAll<Suit>("Suits").ToList();
        _faces = Resources.LoadAll<Face>("Faces").ToList();
    }
    public Card GetCard(string cardString)
    {
        var suitLetter = cardString.Last().ToString();
        var numeral = cardString.Substring(0, cardString.Length - 1).Trim();
        var suit = _suits.SingleOrDefault(s => s.shortName == suitLetter);
        var value = 10;
        Face face = null;
        if(int.TryParse(numeral, out var parsed)) value = parsed;
        else face = _faces.Single(f => f.numeral == numeral);
        return new Card(suit, value, face);
    }

    public List<Card> GetCards(string decklist)
    {
        var cardStrings = decklist.RemoveWhitespace().Split(';');
        var cards = cardStrings.Where(s => !string.IsNullOrWhiteSpace(s) && s.Length > 1).Select(cs => GetCard(cs));
        return cards.ToList();
    }

    public List<Card> GetRandomCards(int count)
    {
        var numerals = Enumerable.Range(0, _highestNumeral+1).Select(n => n.ToString()).ToList();
        var faceNumerals = _faces.Select(f => f.numeral).ToList();
        numerals.AddRange(faceNumerals);
        var suitLetters = _suits.Select(s => s.shortName).Append("-").ToList();
        var cards = new List<Card>();
        for(int i = 0; i < count; i++)
        {
            var numeral = numerals[URandom.Range(0, numerals.Count)];
            var suitLetter = suitLetters[URandom.Range(0,suitLetters.Count)];
            cards.Add(GetCard(numeral + suitLetter));
        }
        return cards;
    }
}