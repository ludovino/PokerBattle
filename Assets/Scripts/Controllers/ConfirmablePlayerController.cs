using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ConfirmablePlayerController : PlayerController
{
    [SerializeField]
    private Entity _playerEntity;
    [SerializeField]
    private Hand _hand;
    [SerializeField]
    private DropTarget _handDropzone;
    private CardScript[] toPlay;
    private TextMeshProUGUI _endTurnButtonText;

    private bool cardsToPlay => toPlay.Any(x => x != null);
    private bool buttonInteractable => battle.CanEndTurn() || cardsToPlay;
    public new void Awake()
    {
        toPlay ??= new CardScript[5];
        _handDropzone.onDrop.AddListener(ReturnToHand);
        base.Awake();
        _endTurnButtonText = _endTurnButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public override bool Play(CardSlot cardSlot, CardScript card)
    {
        if(!enabled || !_playerEntity.CanPlay(cardSlot.slotNumber, card)) return false;
        
        var currentIndex = Array.IndexOf(toPlay, card);
        if (currentIndex >= 0) toPlay[currentIndex] = null;

        _playerEntity.hand.Remove(card);
        card.transform.DOMove(cardSlot.transform.position + Vector3.down * 0.2f, 0.2f).SetEase(Ease.OutExpo);

        ReturnToHand(cardSlot.slotNumber);

        _hand.AlignHand();
        toPlay[cardSlot.slotNumber] = card;

        _endTurnButton.interactable = true;
        SetButton();
        return true;
    }


    public void ReturnToHand(int slot)
    {
        var currentCard = toPlay[slot];
        if (currentCard == null) return;
        toPlay[slot] = null;
        _playerEntity.hand.Add(currentCard);
        SetButton();
    }

    private void SetButton()
    {
        if(cardsToPlay)
        {
            _endTurnButtonText.text = "Play Cards";
            return;
        }
        _endTurnButtonText.text = "End Turn";
    }

    public void ReturnToHand(Draggable cardDraggable)
    {
        var card = cardDraggable.GetComponent<CardScript>();
        
        var currentIndex = Array.IndexOf(toPlay, card);
        if (card == null || currentIndex == -1)
        {
            cardDraggable.Return();
            return;
        }
        ReturnToHand(currentIndex);
        SetButton();
        _hand.AlignHand();
    }

    public override void PlayerEndTurn()
    {
        _endTurnButton.interactable = false;
        if (!cardsToPlay)
        {
            base.PlayerEndTurn();
        }
        else
        {
            for (int i = 0; i < toPlay.Length; i++)
            {
                var card = toPlay[i];
                if (card == null) continue;
                battle.Play(i, card);
                toPlay[i] = null;
            }
            if(_playerEntity.slotsRemaining == 0)
            {
                base.PlayerEndTurn();
            }
        }
        CoroutineQueue.Defer(CR_ResetButton());
    }


    private IEnumerator CR_ResetButton()
    {
        yield return null;
        SetButton();
        if (_endTurnButton) _endTurnButton.interactable = buttonInteractable;
    }
}

