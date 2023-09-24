using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;
using NaughtyAttributes;

public class Entity : MonoBehaviour
{
    //TODO: make private
    public EntityController controller;
    public int blind;
    public int chips;
    public EntityData entityData;
    private List<CardScript> _drawPileCards;
    public GameObject cardPrefab;
    public List<CardScript> hand;
    private List<CardScript> _discardPileCards;
    public List<CardScript> removePile;
    public CardScript[] fieldOfPlay;

    #region events
    [Foldout("Events")]
    public OnDraw onDraw;
    [Foldout("Events")]
    public OnPlay onPlay;
    [Foldout("Events")]
    public OnDiscard onDiscard;
    [Foldout("Events")]
    public OnFieldClear onFieldClear;
    [Foldout("Events")]
    public OnChangeChips onChangeChips;
    [Foldout("Events")]
    public UnityEvent allIn;
    #endregion

    public List<Card> played => fieldOfPlay.Where(c => c != null).Select(c => c.card).ToList();
    private BattleController _battleController;

    public void Awake()
    {
        InitializeCollections();
        onDraw ??= new OnDraw();
        onPlay ??= new OnPlay();
        onDiscard ??= new OnDiscard();
        onFieldClear ??= new OnFieldClear();
        onChangeChips ??= new OnChangeChips();
        allIn ??= new UnityEvent();
    }
    public void Start()
    {
        _battleController = FindObjectOfType<BattleController>();
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
            cardObj.SetCard(card, entityData);
            cardObj.gameObject.SetActive(false);
            _drawPileCards.Add(cardObj);
        }
    }

    internal void Init(EntityData entityData, int blind)
    {
        this.entityData = entityData ?? this.entityData;
        this.blind = blind;
        Init();
    }
    
    private void InitializeCollections()
    {
        fieldOfPlay = new CardScript[5];
        _drawPileCards = new List<CardScript>();
        _discardPileCards = new List<CardScript>();
        removePile = new List<CardScript>();
    }

    public RankedHand Evaluate()
    {
        return entityData.HandList.Evaluate(fieldOfPlay.ToList());
    }

    public RankedHand Evaluate(List<CardScript> cards)
    {
        return entityData.HandList.Evaluate(cards);
    }


    public void OnTurnStart()
    {
        var onStartCards = fieldOfPlay.Where(c => c != null);
        foreach (var card in onStartCards)
        {
            card.DoPlayerTurnEffects();
        }
    }

    public void OnOpponentTurnStart()
    {
        var onStartCards = fieldOfPlay.Where(c => c != null);
        foreach (var card in onStartCards)
        {
            card.DoOpponentTurnEffects();
        }
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
        if(CanPlay(slotNumber, card))
        {
            fieldOfPlay[slotNumber] = card;
            hand.Remove(card);
            onPlay.Invoke(card, slotNumber);
            return true;
        }
        return false;
    }

    public bool CanPlay(int slotNumber, CardScript card)
    {
        if (slotNumber >= fieldOfPlay.Length) throw new Exception($"tried to play card in slot {slotNumber} on a field of size {fieldOfPlay.Length}");
        return CanOverwrite(card, fieldOfPlay[slotNumber]);
    }

    public bool CanOverwrite(CardScript toPlay, CardScript current)
    {
        if(current == null) return true;
        //overwriting logic
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
            _discardPileCards = new List<CardScript>();
            _drawPileCards.Shuffle();
        }
        var drawn = _drawPileCards.Take(toDraw).ToList();
        foreach(var card in drawn){
            card.Draw(new CardEffectContext(_battleController, this, null, card));
            _drawPileCards.Remove(card);
            hand.Add(card);
        }
        onDraw.Invoke(drawn);
    }

    public void DiscardHand()
    {
        if (!hand.Any()) return;
        _discardPileCards.AddRange(hand);
        foreach(var card in hand)
        {
            card.Discard();
        }
        onDiscard.Invoke(hand);
        hand.Clear();
    }
    
    public void ClearField()
    {
        onFieldClear.Invoke(fieldOfPlay);
        var toDiscard = fieldOfPlay.Where(c => c != null).ToList();
        _discardPileCards.AddRange(toDiscard);
        foreach (var card in toDiscard)
        {
            card.Discard();
        }
        onDiscard.Invoke(fieldOfPlay.ToList());
        for (int i = 0; i < fieldOfPlay.Length; i++)
        {
            fieldOfPlay[i] = null;
        }
    }

    internal void AllIn()
    {
        allIn.Invoke();
    }

    internal void OpponentPlayed(int slotNumber)
    {
        var card = fieldOfPlay[slotNumber];
        if (card is null) return;
        card.DoOpponentPlayEffects();
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

public class DrawContext
{
    private Entity _entity;
    private CardScript _card;
    private BattleController _battle;
    public Entity Entity => _entity;
    public CardScript Card => _card;
    public BattleController Battle => _battle;
    public DrawContext(BattleController battle, Entity entity, CardScript card)
    {
        _entity = entity;
        _card = card;
        _battle = battle;
    }
}