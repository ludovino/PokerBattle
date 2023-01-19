using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using URandom = UnityEngine.Random;
[CreateAssetMenu(menuName = "Card/Pool")]
public class CardPool : ScriptableObject
{
    private List<Suit> _suitList;
    private IReadOnlyList<Suit> suits => GetSuits();
    [SerializeField]
    private SuitList _suits;
    [SerializeField]
    private SuitList _except;

    private IReadOnlyList<Suit> GetSuits()
    {
        if (_except is null || _except.Count == 0) return _suits;
        if (_suitList is null) _suitList = new List<Suit>();
        var suitsExcept = _suits.Except(_except).ToList();
        if (!_suitList.SequenceEqual(suitsExcept))
        {
            _suitList.Clear();
            _suitList.AddRange(suitsExcept);
        }
        return _suitList;
    }
    private void OnEnable()
    {
        GetSuits();
    }

    public List<Card> GetWithReplacement(int count)
    {
        var cardList = new List<Card>();
        var numeralList = GetNumerals();
        for(int i = 0; i < count; i++)
        {
            var suit = suits[URandom.Range(0, suits.Count)];
            var numeral = numeralList[URandom.Range(0, numeralList.Count)];
            var card = CardFactory.Instance.GetCard(numeral + SuitName(suit));
            cardList.Add(card);
        }
        return cardList;
    }

    public Card GetOne()
    {
        var numeralList = GetNumerals(); 
        var suit = suits[URandom.Range(0, suits.Count)];
        var numeral = numeralList[URandom.Range(0, numeralList.Count)];
        var card = CardFactory.Instance.GetCard(numeral + SuitName(suit));
        return card;
    }

    private string SuitName(Suit suit) => suit?.shortName ?? "-";
    
    public List<Card> GetWithoutReplacement(int count, bool eachSuit)
    {
        var numeralList = GetNumerals();
        var cardList = new List<string>();
        if (eachSuit)
        {
            cardList = suits.SelectMany(s => numeralList.Select(n => n + SuitName(s))).ToList();
        }
        else
        {
            cardList = numeralList.Select(n => n + SuitName(suits[URandom.Range(0, suits.Count)])).ToList();
        }
        cardList.Shuffle();
        return cardList.Take(count).Select(s => CardFactory.Instance.GetCard(s)).ToList();
    }

    private List<string> GetNumerals()
    {
        var numeralList = new List<string>();
        foreach (var nc in numerals)
        {
            var numeralStrings = Enumerable.Range(nc.first, nc.last - nc.first + 1).Select(n => n.ToString());
            for(int i = 0; i < nc.Count; i++)
            {
                numeralList.AddRange(numeralStrings);
            }
        }

        foreach(var fc in faces)
        {
            for (int i = 0; i < fc.Count; i++)
            {
                numeralList.Add(fc.Face.numeral);
            }
        }
        return numeralList;
    }
    
    public List<NumeralCount> numerals;
    public List<FaceCount> faces;
    [Serializable]
    public class FaceCount
    {
        public Face Face;
        public int Count;
    }
    [Serializable]
    public class NumeralCount
    {
        public int first;
        public int last;
        public int Count;
    }
}



