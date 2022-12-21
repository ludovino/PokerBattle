using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    private CardCollection _cardCollection;

    private void Awake()
    {
        _cardCollection = GetComponent<CardCollection>();
    }

    public void AddCards(List<CardScript> toAdd)
    {
        var cards = toAdd.Select(c => c.gameObject).ToList();
        CoroutineQueue.Defer(_cardCollection.AddCards(cards));
    }

    public void RemoveCards(List<CardScript> toRemove)
    {
        var cards = toRemove.Select(c => c.gameObject).ToList();
        CoroutineQueue.Defer(_cardCollection.RemoveCards(cards));
    }
}
