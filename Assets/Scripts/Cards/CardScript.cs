using Assets.Scripts.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardScript : MonoBehaviour, ICard
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private CardSpriteCollection _spriteCollection;
    public UnityEvent onPlay;
    public UnityEvent onDraw;
    public UnityEvent onDiscard;
    private Card _card;
    private Card _tempCard;
    private Draggable _draggable;
    private CardEffectContext _playContext;
    private EntityData _owner;
    private EntityData _holder;
    private List<CardEffect> _cardEffects = new List<CardEffect>(32);
    public bool Draggable { get { return _draggable.enabled; } set { _draggable.enabled = value; } }
    public void SetCard(Card card, EntityData owner)
    {
        _card = card;
        _tempCard = _card.Clone();
        gameObject.name = card.ToString();
        _owner = owner;
        _holder = owner;
        UpdateSprite();
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
    private List<GameObject> _previewEffects = new List<GameObject>();
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

    public void Preview(CardEffectContext context)
    {
        _playContext = context;
        DoPreviewEffects();
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
        ResetCard();
        onDiscard.Invoke();
    }

    public void ResetCard()
    {
        _tempCard.Set(_card);
        UpdateEffects();
        UpdateSprite();
        SetTooltip();
    }
    public IEnumerator ResetCardAnimated()
    {
        _tempCard.Set(_card);
        UpdateEffects();
        var sprite = _spriteCollection.Get(_tempCard);
        SetTooltip();
        return CR_UpdateSprite(sprite);
    }

    public IEnumerator ChangeValue(int change)
    {
        if (change == 0) return null;
        _tempCard.Change(change); 
        var sprite = _spriteCollection.Get(_tempCard);
        UpdateEffects();
        return CR_UpdateSprite(sprite);
    }

    public IEnumerator ChangeSuit(Suit suit)
    {
        if (suit == this.suit) return null;
        _tempCard.SetSuit(suit);
        var sprite = _spriteCollection.Get(_tempCard);
        UpdateEffects();
        return CR_UpdateSprite(sprite);
    }


    private IEnumerator CR_UpdateSprite(Sprite sprite)
    {
        UpdateSprite(sprite);
        yield return null;
    }

    private void UpdateSprite(Sprite sprite = null)
    {
        if(sprite == null) sprite = _spriteCollection.Get(_tempCard);
        if(_spriteRenderer) _spriteRenderer.sprite = sprite;
        if(_image) _image.sprite = sprite;
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

    public void DoPreviewEffects()
    {
        foreach (var effect in _cardEffects)
        {
            if (effect is IOnHoverEnter e)
            {
                _previewEffects.Add(e.OnHoverEnter(_playContext));
            }
        }
    }

    public void ClearPreviews()
    {
        for (var i = _previewEffects.Count - 1; i >= 0; i--)
        {
            Destroy(_previewEffects[i]);
        }
        _previewEffects.Clear();
    }
}
