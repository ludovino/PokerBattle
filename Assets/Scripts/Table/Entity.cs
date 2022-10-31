using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    //TODO: make private
    public EntityController controller;
    public int blind;
    public int chips;
    public EntityData entityData;
    private List<CardScript> _drawPile;
    public GameObject cardPrefab;
    public List<CardScript> hand;
    private List<CardScript> _discardPile;
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
            cardObj.gameObject.SetActive(false);
            _drawPile.Add(cardObj);
        }
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
        _drawPile = new List<CardScript>();
        _discardPile = new List<CardScript>();
        removePile = new List<CardScript>();
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

    public IEnumerable<Card> DrawPile => _drawPile.Select(c => c.card);
    public IEnumerable<Card> DiscardPile => _discardPile.Select(c => c.card);

    public void Draw()
    {
        var count = Mathf.Max(slotsRemaining + entityData.DrawBonus, 1);
        Draw(count);
    }
    public void Draw(int number)
    {
        var toDraw = number;

        if(_drawPile.Count < number)
        {
            toDraw -= _drawPile.Count;
            Draw(_drawPile.Count);
            _drawPile = _discardPile;
            _discardPile = new List<CardScript>();
            _drawPile.Shuffle();
        }
        var drawn = _drawPile.Take(toDraw).ToList();
        foreach(var card in drawn){
            card.Draw();
            _drawPile.Remove(card);
            hand.Add(card);
        }
        onDraw.Invoke(drawn);
    }

    public void DiscardHand()
    {
        _discardPile.AddRange(hand);
        onDiscard.Invoke(hand);
        hand.Clear();
    }
    public void ClearField()
    {
        onFieldClear.Invoke(fieldOfPlay);
        _discardPile.AddRange(fieldOfPlay.Where(c => c != null));
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