using UnityEngine;
using TMPro;
using UnityEngine.Events;
using TMPro.EditorUtilities;
using System;

public class CardScript : MonoBehaviour, ICard
{
    [SerializeField]
    private CardDisplay _cardDisplay;
    public UnityEvent onPlay;
    public UnityEvent onDraw;
    public UnityEvent onDiscard;
    private Card _card;
    private Card _tempCard;
    private Draggable _draggable;
    private PlayContext _playContext;

    public bool Draggable { get { return _draggable.enabled; } set { _draggable.enabled = value; } }
    public void SetCard(Card card)
    {
        _card = card;
        _tempCard = _card.Clone();
        gameObject.name = card.ToString();
        _cardDisplay.UpdateCardDisplay(this);
        SetTooltip();
    }
    public Card card => _tempCard ?? _card;
    public int highCardRank => card.highCardRank;
    public int blackjackValue => card.blackjackValue;
    public string numeral => card.numeral;
    public Suit suit => card.suit;
    public Face face => card.face;
    public CardEffect effect => suit?.CardEffect;
    public PlayContext playContext => _playContext;
    public int valueDifference => _tempCard.highCardRank - _card.highCardRank;
    private SimpleTooltip _tooltip;

    void Awake()
    {
        onPlay = onPlay ?? new UnityEvent();
        onDraw = onDraw ?? new UnityEvent();
        onDiscard = onDiscard ?? new UnityEvent();
        _draggable = GetComponent<Draggable>();
        _tooltip = GetComponent<SimpleTooltip>();
    }

    void LateUpdate()
    {
        if (!_cardDisplay.FaceUp) _tooltip.HideTooltip();
    }

    public void Play(PlayContext context)
    {
        _playContext = context;
        if (effect is IOnPlay) ExecuteEffect();
        onPlay.Invoke();
    }
    public void Draw()
    {
        _playContext = null;
        onDraw.Invoke();
    }
    public void Discard()
    {
        _playContext = null;
        onDiscard.Invoke();
    }
    public void ResetCard(bool animate = false)
    {
        _tempCard = _card.Clone();
        if (animate) _cardDisplay.AnimateCardDisplay(_tempCard);
        else _cardDisplay.UpdateCardDisplay(_tempCard);
    }
    public void ChangeValue(int change)
    {
        if (change == 0) return;
        _tempCard.Change(change);
        _cardDisplay.AnimateCardDisplay(_tempCard);
    }

    public void ChangeSuit(Suit suit)
    {
        if (suit == this.suit) return;
        _tempCard.SetSuit(suit);
        _cardDisplay.AnimateCardDisplay(_tempCard);
    }

    internal void ExecuteEffect()
    {
        effect.Execute(_playContext);
    }

    private void SetTooltip()
    {
        var numeralName = face?.longName ?? (highCardRank > 0 ? highCardRank.ToString() : "Nil");
        var suitName = suit?.longName ?? "Nothing";
        var name = $"{numeralName} of {suitName}";

        var effectDescription = suit?.CardEffect.Description;
        _tooltip.infoLeft = $"~{name}" +
            $"\n@{effectDescription}";
    }
}
