using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardScript : MonoBehaviour, ICard
{
    [SerializeField]
    private TweenerCanvasCardDisplay _cardDisplay;
    public UnityEvent onPlay;
    public UnityEvent onDraw;
    public UnityEvent onDiscard;
    private Card _card;
    private Card _tempCard;
    private Draggable _draggable;
    private CardEffectContext _playContext;
    private EntityData _owner;
    private EntityData _holder;
    private List<CardEffect> _cardEffects;
    [SerializeField]
    private float _animationTime = 0.3f;
    public bool Draggable { get { return _draggable.enabled; } set { _draggable.enabled = value; } }
    public void SetCard(Card card, EntityData owner)
    {
        _card = card;
        _tempCard = _card.Clone();
        gameObject.name = card.ToString();
        _cardDisplay.Set(blackjackValue, suit, face, true);
        _owner = owner;
        _holder = owner;
        UpdateEffects();
        SetTooltip();
    }
    public Card card => _tempCard ?? _card;
    public int highCardRank => card.highCardRank;
    public int blackjackValue => card.blackjackValue;
    public string numeral => card.numeral;
    public Suit suit => card.suit;
    public Face face => card.face;
    public CardEffectContext playContext => _playContext;
    public int valueDifference => _tempCard.highCardRank - _card.highCardRank;
    private SimpleTooltip _tooltip;
    private CardBackScript _cardBack;

    void Awake()
    {
        onPlay = onPlay ?? new UnityEvent();
        onDraw = onDraw ?? new UnityEvent();
        onDiscard = onDiscard ?? new UnityEvent();
        _draggable = GetComponent<Draggable>();
        _tooltip = GetComponent<SimpleTooltip>();
        _cardBack = GetComponent<CardBackScript>();
        _cardEffects = new List<CardEffect>(32);
    }

    void LateUpdate()
    {
        if (_cardBack && !_cardBack.faceUp) _tooltip.HideTooltip();
    }

    public void UpdateEffects(ICard card = null)
    {
        if (card is null) card = _tempCard;
        foreach(var cardEffect in _cardEffects)
        {
            if (!cardEffect.Condition(card)) cardEffect.OnEffectRemove(_playContext);
        }
        _cardEffects.Clear();
        _cardEffects.AddRange(_holder.EffectList.ForCard(card));
    }

    public void Play(CardEffectContext context)
    {
        _playContext = context;
        onPlay.Invoke();
        DoPlayEffects();
    }

    public void Draw(CardEffectContext context)
    {
        _playContext = context;
        onDraw.Invoke();
        DoDrawEffects();
    }

    public void Discard()
    {
        _playContext = null;
        ResetCard(0.0f);
        onDiscard.Invoke();
    }

    public void ResetCard()
    {
        _tempCard = _card.Clone();
        CR_AnimateCard(_tempCard);
    }
    public void ResetCard(float time)
    {
        _tempCard = _card.Clone();
        CR_AnimateCard(_tempCard, time);
    }
    public void ResetCardImmediate()
    {
        _cardDisplay.Set(_tempCard, true); 
        SetTooltip();
    }

    public IEnumerator ChangeValue(int change)
    {
        if (change == 0) return null;
        _tempCard.Change(change);
        UpdateEffects();
        return CR_AnimateCard(_tempCard);
    }

    public IEnumerator ChangeSuit(Suit suit)
    {
        if (suit == this.suit) return null;
        _tempCard.SetSuit(suit);
        UpdateEffects();
        return CR_AnimateCard(_tempCard);
    }

    private IEnumerator CR_AnimateCard(Card card)
    {
        SetTooltip();
        yield return _cardDisplay.CR_Animate(card, _animationTime);
    }

    private IEnumerator CR_AnimateCard(Card card, float time)
    {
        SetTooltip();
        yield return _cardDisplay.CR_Animate(card, time);
    }

    private void SetTooltip()
    {
        if(_tooltip == null) _tooltip = GetComponent<SimpleTooltip>();
        var numeralName = face?.longName ?? (highCardRank > 0 ? highCardRank.ToString() : "Nil");
        var suitName = suit?.longName ?? "Nothing";
        var name = $"{numeralName} of {suitName}";
        if (highCardRank == 0 && suit == null) name = "blank";
        var effectDescription = _owner.EffectList.CardEffectDescription(this);
        _tooltip.infoLeft = $"~{name}" +
            $"\n@{effectDescription}";
    }
    public void DoWinEffects()
    {
        foreach (var effect in _cardEffects)
        {
            if (effect is IOnWinHand e)
            {
                e.OnWinHand(_playContext);
            }
        }
    }
    public void DoPlayEffects()
    {
        foreach (var effect in _cardEffects)
        {
            if (effect is IOnPlay e)
            {
                e.OnPlay(_playContext);
            }
        }
    }
    public void DoDrawEffects()
    {
        foreach (var effect in _cardEffects)
        {
            if (effect is IOnDraw e)
            {
                e.OnDraw(_playContext);
            }
        }
    }
    public void DoPlayerTurnEffects()
    {
        foreach (var effect in _cardEffects)
        {
            if (effect is IOnPlayerTurn e)
            {
                e.OnPlayerTurn(_playContext);
            }
        }
    }
    public void DoOpponentTurnEffects()
    {
        foreach (var effect in _cardEffects)
        {
            if (effect is IOnOpponentTurn e)
            {
                e.OnOpponentTurn(_playContext);
            }
        }
    }
    public void DoOpponentPlayEffects()
    {
        foreach (var effect in _cardEffects)
        {
            if (effect is IOnOpponentPlay e)
            {
                e.OnOpponentPlay(_playContext);
            }
        }
    }
}
