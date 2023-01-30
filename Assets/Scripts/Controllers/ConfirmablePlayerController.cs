using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmablePlayerController : PlayerController
{
    [SerializeField]
    private Entity _playerEntity;
    [SerializeField]
    private Hand _hand;
    [SerializeField]
    private DropTarget _handDropzone;
    private CardScript[] toPlay;
    [SerializeField]
    private Button _setCardsButton;

    private bool cardsToPlay => toPlay.Any(x => x != null);
    private bool endTurnInteractable => battle.CanEndTurn() || cardsToPlay;

    public new void Awake()
    {
        toPlay ??= new CardScript[5];
        _handDropzone.onDrop.AddListener(ReturnToHand);
        base.Awake();
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
        UpdateButtons();
        return true;
    }


    public void ReturnToHand(int slot)
    {
        var currentCard = toPlay[slot];
        if (currentCard == null) return;
        toPlay[slot] = null;
        _playerEntity.hand.Add(currentCard);
        UpdateButtons();
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
        UpdateButtons();
        _hand.AlignHand();
    }

    public override void PlayerEndTurn()
    {
        _endTurnButton.interactable = false;
        if (cardsToPlay)
        {
            SetCards(true);
        }
        else
        {
            base.PlayerEndTurn();
        }

        CoroutineQueue.Defer(CR_ResetButton());
    }
    public void SetCards(bool endTurn)
    {
        for (int i = 0; i < toPlay.Length; i++)
        {
            var card = toPlay[i];
            if (card == null) continue;
            battle.Play(i, card);
            toPlay[i] = null;
        }
        if (_playerEntity.slotsRemaining == 0 || endTurn)
        {
            base.PlayerEndTurn();
        }
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        if(_endTurnButton) _endTurnButton.interactable = endTurnInteractable;
        if(_setCardsButton) _setCardsButton.interactable = cardsToPlay;
    }

    private IEnumerator CR_ResetButton()
    {
        yield return null;
        UpdateButtons();
    }
}

