using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using static Animancer.Strings;

public class CardSelector : MonoBehaviour
{
    [SerializeField]
    private OnSelectCard _onSelectCard;
    public OnSelectCard OnSelectCard => _onSelectCard;
    [SerializeField]
    private OnDeselectCard _onDeselectCard;
    public OnDeselectCard OnDeselectCard => _onDeselectCard;

    [SerializeField]
    private CardDisplay _cardDisplay;
    private SimpleTooltip _tooltip;
    private Card _card;
    private bool _selected;
    public Card Card => _card;

    private void Awake()
    {
        _onSelectCard = OnSelectCard ?? new OnSelectCard();
        _onDeselectCard = _onDeselectCard ?? new OnDeselectCard();
        _tooltip = GetComponent<SimpleTooltip>();
        _selected = false;
    }

    private void OnMouseDown()
    {
        if (!_selected) Select();
        else Deselect();
    }

    public void Deselect()
    {
        _onDeselectCard.Invoke(this);
        transform.DOScale(Vector3.one, 0.2f);
        _cardDisplay.GlowOff();
        _selected = false;
    }

    public void Select()
    {
        _onSelectCard.Invoke(this);
        transform.DOScale(Vector3.one * 1.1f, 0.2f);
        _cardDisplay.Glow(0.3f, Color.cyan);
        _selected = true;
    }

    public void SetCard(Card card)
    {
        _card = card;
        _cardDisplay.UpdateCardDisplay(_card);
        SetTooltip();
    }


    private void SetTooltip()
    {
        var numeralName = _card.face?.longName ?? (_card.highCardRank > 0 ? _card.highCardRank.ToString() : "Nil");
        var suitName = _card.suit?.longName ?? "Blank";
        var name = $"{numeralName} of {suitName}";

        var effectDescription = _card.suit?.CardEffect.Description;
        _tooltip.infoLeft = $"~{name}" +
            $"\n@{effectDescription}";
    }

    private void OnDestroy()
    {
        _onSelectCard.RemoveAllListeners();
    }
}
[Serializable]
public class OnSelectCard : UnityEvent<CardSelector> { }
[Serializable]
public class OnDeselectCard : UnityEvent<CardSelector> { }