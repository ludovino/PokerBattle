using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : EntityController
{
    [SerializeField]
    private CardSelectMenu _cardSelect;
    [SerializeField]
    protected Button _endTurnButton;
    [SerializeField]
    private CardSlot[] _cardSlots;
    [SerializeField]
    private CardFan _cardFan;
    private GameObject _highlighted;
    private GameObject _selected;
    
    public override void ChooseCards(List<Card> cards, int count, Action<List<Card>> selectCallback)
    {
        StartCoroutine(CR_ChooseCard(cards, count, selectCallback));
    }

    public void Start()
    {
        for (int i = 0; i < _cardSlots.Length; i++)
        {
            CardSlot cardSlot = _cardSlots[i];
            cardSlot.OnCardDrop += Play;
            cardSlot.SlotNumber = i;
        }
    }

    public override void Init(){}
    public override void StartTurn()
    {
        base.StartTurn();
        foreach (var cardSlot in _cardSlots)
        {
            cardSlot.EnableDropTarget();
        }
        _endTurnButton.interactable = battle.CanEndTurn();
    }

    public virtual bool Play(CardSlot cardSlot, CardScript card)
    {
        if(!enabled) return false;
        var result = battle == null || battle.Play(cardSlot.SlotNumber, card);
        if (result)
        {
            card.Draggable = false;
            _cardFan.RemoveCard(card.gameObject);
        }
        else
        {
            _cardFan.ReturnCard(card.gameObject);    
        }
        _endTurnButton.interactable = battle.CanEndTurn();
        return result;
    }

    private IEnumerator CR_ChooseCard(List<Card> cards, int count, Action<List<Card>> selectCallback)
    {
        var selected = new List<Card>();
        var chosen = false;
        _cardSelect.Init(cards, count);
        _cardSelect.StartSelect();
        UnityAction<List<CardScript>> select = c => 
        {
            selected = c.Select(c => c.card).ToList();
            chosen = true;
        };
        _cardSelect.OnSelect.AddListener(select);
        yield return new WaitUntil(() => chosen);
        _cardSelect.OnSelect.RemoveListener(select);
        selectCallback.Invoke(selected);
    }

    public virtual void PlayerEndTurn()
    {

        if (!battle.CanEndTurn()) return;
        _endTurnButton.interactable = false;
        foreach (var cardSlot in _cardSlots)
        {
            cardSlot.DisableDropTarget();
        }
        EndTurn();
    }
}