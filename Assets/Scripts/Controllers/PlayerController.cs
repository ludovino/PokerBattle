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
    private Button _endTurnButton;
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
    public bool Play(int slotNumber, CardScript card)
    {
        if(!enabled) return false;
        var result = battle.Play(slotNumber, card);
        if (result) card.Draggable = false;
        _endTurnButton.interactable = battle.CanEndTurn();
        return result;
    }

    private IEnumerator CR_ChooseCard(List<Card> cards, int count, Action<List<Card>> selectCallback)
    {
        var selected = new List<Card>();
        var chosen = false;
        _cardSelect.StartSelect(cards, count);
        UnityAction<List<CardSelector>> select = c => 
        {
            selected = c.Select(cs => cs.Card).ToList();
            chosen = true;
        };
        _cardSelect.OnSelect.AddListener(select);
        yield return new WaitUntil(() => chosen);
        _cardSelect.OnSelect.RemoveListener(select);
        selectCallback.Invoke(selected);
    }
}