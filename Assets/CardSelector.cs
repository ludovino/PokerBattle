using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

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
    private Card _card;
    private bool _selected;
    public Card Card => _card;

    private void Awake()
    {
        _onSelectCard = OnSelectCard ?? new OnSelectCard();
        _onDeselectCard = _onDeselectCard ?? new OnDeselectCard();
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