using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "HandList")]
public class HandList : ScriptableObject
{
    [SerializeField]
    private List<PokerHand> hands;
    public IReadOnlyList<PokerHand> Hands => hands.Append(fallback).Distinct().ToList();
    private HighCard fallback;
    void OnEnable()
    {
        hands = hands ?? new List<PokerHand>();
        fallback = Resources.Load<HighCard>("Hands/HighCard");
    }
    public RankedHand Evaluate(List<CardScript> cardScripts)
    {
        var rankedHands = hands.OrderByDescending(h => h.rank).ToList();
        foreach(var hand in rankedHands)
        {
            if(hand.Evaluate(cardScripts.Cast<ICard>().ToList())) return hand.GetRankedHand(cardScripts);
        }
        return fallback.GetRankedHand(cardScripts);
    }
}
