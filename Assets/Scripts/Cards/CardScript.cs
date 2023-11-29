using Assets.Scripts.Cards;
using DG.Tweening;
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
    [SerializeField]
    private Card _card;
    private Card _tempCard;
    private Draggable _draggable;
    private CardEffectContext _playContext;
    private EntityData _owner;
    private EntityData _holder;
    public bool Draggable { get { return _draggable.enabled; } set { _draggable.enabled = value; } }
    public void SetCard(Card card, EntityData owner)
    {
        _card = card;
        _tempCard = _card.Clone();
        gameObject.name = card.ToString();
        _owner = owner;
        _holder = owner;
        UpdateSprite();
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
    }

    void LateUpdate()
    {
        if (_cardBack && !_cardBack.faceUp) _tooltip.HideTooltip();
    }

    private void Start() {
        if(_tempCard == null){
            _tempCard = _card;
        }

        if(card != null){
            UpdateSprite();
        }
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
        UpdateSprite();
        SetTooltip();
    }
    public Sprite ResetCardAnimated()
    {
        _tempCard.Set(_card);
        var sprite = _spriteCollection.Get(_tempCard);
        SetTooltip();
        return sprite;
    }

    public Sprite ChangeValue(int change)
    {
        if (change == 0) return null;
        _tempCard.Change(change); 
        var sprite = _spriteCollection.Get(_tempCard);
        return sprite;
    }

    public Sprite ChangeSuit(Suit suit)
    {
        if (suit == this.suit) return null;
        _tempCard.SetSuit(suit);
        var sprite = _spriteCollection.Get(_tempCard);
        return sprite;
    }
    public void SetSprite(Sprite sprite)
    {
        UpdateSprite(sprite);
    }
    public void SetSprite(Sprite sprite, Color hit, float fade)
    {
        UpdateSprite(sprite);
        if(_spriteRenderer) _spriteRenderer.DOBlendableColor(hit, fade).From().SetEase(Ease.InCirc);
        if(_image) _image.DOBlendableColor(hit, fade).From().SetEase(Ease.InCirc);
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

    private IEnumerable<T> GetEffects<T>() where T : ICardEffect => _holder.EffectList.ForCard<T>(_tempCard);

    public void DoWinEffects()
    {
        foreach (var effect in GetEffects<IOnWinHand>())
        {
            effect.OnWinHand(_playContext);
        }
    }
    public void DoPlayEffects()
    {
        foreach (var effect in GetEffects<IOnPlay>())
        {
            effect.OnPlay(_playContext);
        }
    }
    public void DoDrawEffects()
    {
        foreach (var effect in GetEffects<IOnDraw>())
        {
            effect .OnDraw(_playContext);
        }
    }
    public void DoPlayerTurnEffects()
    {
        foreach (var effect in GetEffects<IOnPlayerTurn>())
        {
            effect.OnPlayerTurn(_playContext);
        }
    }
    public void DoOpponentTurnEffects()
    {
        foreach (var effect in GetEffects<IOnOpponentTurn>())
        {
            effect.OnOpponentTurn(_playContext);
        }
    }
    public void DoOpponentPlayEffects()
    {
        foreach (var effect in GetEffects<IOnOpponentPlay>())
        {
            effect.OnOpponentPlay(_playContext);
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
