using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : EntityController
{
    [SerializeField]
    private CardSelectMenu _cardSelect;
    [SerializeField]
    protected Button _endTurnButton;
    public override void ChooseCards(List<Card> cards, int count, Action<List<Card>> selectCallback)
    {
        StartCoroutine(CR_ChooseCard(cards, count, selectCallback));
    }
    public override void Init(){}
    public override void StartTurn()
    {
        base.StartTurn();
        _endTurnButton.interactable = battle.CanEndTurn();
    }

    public virtual bool Play(CardSlot cardSlot, CardScript card)
    {
        if(!enabled) return false;
        var result = battle.Play(cardSlot.slotNumber, card);
        if (result) card.Draggable = false;
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
        EndTurn();
    }
}