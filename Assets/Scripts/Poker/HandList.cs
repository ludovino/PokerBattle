using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "HandList")]
public class HandList : ScriptableObject, IOnInit
{
    [SerializeField]
    [FormerlySerializedAs("hands")]
    private List<PokerHand> startingHands;
    private List<PokerHand> _currentHands;
    public IReadOnlyList<PokerHand> Hands => _currentHands.Append(fallback).Distinct().ToList();
    private HighCard fallback;
    
    void OnEnable()
    {
        startingHands ??= new List<PokerHand>();
        fallback = Resources.Load<HighCard>("Hands/HighCard");
    }


    public RankedHand Evaluate(List<CardScript> cardScripts)
    {
        var rankedHands = _currentHands.OrderByDescending(h => h.rank).ToList();
        foreach(var hand in rankedHands)
        {
            if(hand.Evaluate(cardScripts.Cast<ICard>().ToList())) return hand.GetRankedHand(cardScripts);
        }
        return fallback.GetRankedHand(cardScripts);
    }

    public void AddHands(List<PokerHand> pokerHands)
    {
        _currentHands.AddRange(pokerHands.Except(_currentHands));
    }

    internal void Replace(PokerHand toRemove, PokerHand toAdd)
    {
        if(_currentHands.Remove(toRemove)) _currentHands.Add(toAdd);
    }

    public void Init()
    {
        _currentHands = startingHands;
    }
}
