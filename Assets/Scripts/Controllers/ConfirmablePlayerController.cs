using DG.Tweening;
using System;
using System.Linq;
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
        card.transform.DOMove(cardSlot.transform.position + Vector3.up * 0.2f, 0.2f).SetEase(Ease.OutExpo);

        ReturnToHand(cardSlot.slotNumber);

        _hand.AlignHand();
        toPlay[cardSlot.slotNumber] = card;

        _endTurnButton.interactable = toPlay.Any(x => x != null);

        return true;
    }

    public void ReturnToHand(int slot)
    {
        var currentCard = toPlay[slot];
        if (currentCard == null) return;
        toPlay[slot] = null;
        _playerEntity.hand.Add(currentCard);
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

        _hand.AlignHand();
    }

    public override void PlayerEndTurn()
    {
        for(int i = 0; i < toPlay.Length; i++)
        {
            var card = toPlay[i];
            if (card == null) continue;
            battle.Play(i, card);
            toPlay[i] = null;
        }
        base.PlayerEndTurn();
    }
}

