using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

public class Entity : MonoBehaviour
{
    //TODO: make private
    public EntityController controller;
    public int blind;
    public int chips;
    public EntityData entityData;
    [SerializeField]
    private DrawPile _drawPile;
    private List<CardScript> _drawPileCards;
    public GameObject cardPrefab;
    public List<CardScript> hand;
    [SerializeField]
    private DiscardPile _discardPile;
    private List<CardScript> _discardPileCards;
    public List<CardScript> removePile;
    public CardScript[] fieldOfPlay;
    public OnDraw onDraw;
    public OnPlay onPlay;
    public OnDiscard onDiscard;
    public OnFieldClear onFieldClear;
    public OnChangeChips onChangeChips;
    public UnityEvent allIn;
    public List<Card> played => fieldOfPlay.Where(c => c != null).Select(c => c.card).ToList();

    public void Awake()
    {
        InitializeCollections();
        InitializeEvents();
    }

    private void InitializeEvents()
    {
        onDraw = onDraw ?? new OnDraw();
        onPlay = onPlay ?? new OnPlay();
        onDiscard = onDiscard ?? new OnDiscard();
        onFieldClear = onFieldClear ?? new OnFieldClear();
        onChangeChips = onChangeChips ?? new OnChangeChips();
        allIn = allIn ?? new UnityEvent();
    }

    public void Init()
    {
        InitializeCollections();
        var decklist = entityData.CloneDeck;
        controller.Init();
        decklist.Shuffle();
        ChangeChips(entityData.Chips);
        foreach (var card in decklist)
        {
            var cardObj = Instantiate(cardPrefab).GetComponent<CardScript>();
            cardObj.SetCard(card);
            _drawPileCards.Add(cardObj);
        }

        _drawPile.Init(_drawPileCards);
    }
    internal void Init(EntityData entityData, int blind)
    {
        this.entityData = entityData ?? this.entityData;
        this.blind = blind;
        Init();
    }

    public RankedHand Evaluate()
    {
        return entityData.HandList.Evaluate(fieldOfPlay.ToList());
    }

    public RankedHand Evaluate(List<CardScript> cards)
    {
        return entityData.HandList.Evaluate(cards);
    }

    private void InitializeCollections()
    {
        fieldOfPlay = new CardScript[5];
        _drawPileCards = new List<CardScript>();
        _discardPileCards = new List<CardScript>();
        removePile = new List<CardScript>();
    }

    public void OnTurnStart()
    {
        var onStartCards = fieldOfPlay.Where(c => c?.effect is IOnPlayerTurn).ToList();
        onStartCards.ForEach(c => c.ExecuteEffect());
    }

    public void OnOpponentTurnStart()
    {
        var onStartCards = fieldOfPlay.Where(c => c?.effect is IOnOpponentTurn).ToList();
        onStartCards.ForEach(c => c.ExecuteEffect());
    }

    public void ChangeChips(int change)
    {
        var startingAmount = chips;
        chips += change;
        var currentAmount = chips;
        if (chips != entityData.Chips) entityData.ChangeChips(chips - entityData.Chips);
        onChangeChips.Invoke(startingAmount, currentAmount, change);
    }
    public bool Play(int slotNumber, CardScript card)
    {
        if(slotNumber >= fieldOfPlay.Length) throw new Exception($"tried to play card in slot {slotNumber} on a field of size {fieldOfPlay.Length}");
        if(fieldOfPlay[slotNumber] == null)
        {
            fieldOfPlay[slotNumber] = card;
            hand.Remove(card);
            onPlay.Invoke(card, slotNumber);
            return true;
        }
        return false;
    }
    public int slotsRemaining => fieldOfPlay.Count(c => c == null);

    public IEnumerable<Card> DrawPile => _drawPileCards.Select(c => c.card);
    public IEnumerable<Card> DiscardPile => _discardPileCards.Select(c => c.card);

    public void Draw()
    {
        var count = Mathf.Max(slotsRemaining + entityData.DrawBonus, 1);
        Draw(count);
    }
    public void Draw(int number)
    {
        var toDraw = number;

        if(_drawPileCards.Count < number)
        {
            toDraw -= _drawPileCards.Count;
            Draw(_drawPileCards.Count);
            _drawPileCards = _discardPileCards;
            _drawPile.AddCards(_drawPileCards);
            _discardPileCards = new List<CardScript>();
            _drawPileCards.Shuffle();
        }
        var drawn = _drawPileCards.Take(toDraw).ToList();
        foreach(var card in drawn){
            card.Draw();
            _drawPileCards.Remove(card);
            hand.Add(card);
        }
        _drawPile.RemoveCards(drawn);
        onDraw.Invoke(drawn);
    }

    public void DiscardHand()
    {
        _discardPileCards.AddRange(hand);
        _discardPile.AddCards(hand);
        onDiscard.Invoke(hand);
        hand.Clear();
    }
    public void ClearField()
    {
        onFieldClear.Invoke(fieldOfPlay);
        var toDiscard = fieldOfPlay.Where(c => c != null).ToList();
        _discardPileCards.AddRange(toDiscard);
        _discardPile.AddCards(toDiscard);
        onDiscard.Invoke(fieldOfPlay.ToList());
        fieldOfPlay = new CardScript[5];
    }

    internal void AllIn()
    {
        allIn.Invoke();
    }
}
[Serializable]
public class OnPlay : UnityEvent<CardScript, int>{}
[Serializable]
public class OnDraw : UnityEvent<List<CardScript>>{}
[Serializable]
public class OnDiscard : UnityEvent<List<CardScript>>{}
[Serializable]
public class OnFieldClear : UnityEvent<CardScript[]>{}
[Serializable]
public class OnChangeChips : UnityEvent<int, int, int>{}