using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    private CardCollection _cards;
    private void Awake()
    {
        _cards = GetComponent<CardCollection>();
    }

    public void Init(List<CardScript> drawPileCards)
    {
        var cards = drawPileCards.Select(c => c.gameObject).ToList();
        _cards.AddCardsImmediate(cards);
    }

    public void RemoveCards(List<CardScript> drawn)
    {
        var cards = drawn.Select(c => c.gameObject).ToList();
        CoroutineQueue.Defer(_cards.RemoveCards(cards));
    }

    public void AddCards(List<CardScript> toAdd)
    {
        var cards = toAdd.Select(c => c.gameObject).ToList();
        CoroutineQueue.Defer(_cards.AddCards(cards));
    }
}
