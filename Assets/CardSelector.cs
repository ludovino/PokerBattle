using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardSelector : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private OnSelectCard _onSelectCard;
    public OnSelectCard OnSelectCard => _onSelectCard;
    [SerializeField]
    private OnDeselectCard _onDeselectCard;
    public OnDeselectCard OnDeselectCard => _onDeselectCard;

    private CardScript _cardScript;
    private bool _selected;

    private void Awake()
    {
        _onSelectCard = OnSelectCard ?? new OnSelectCard();
        _onDeselectCard = _onDeselectCard ?? new OnDeselectCard();
        _cardScript = GetComponent<CardScript>();
        _selected = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_selected) Select();
        else Deselect();
    }

    public void Deselect()
    {
        _onDeselectCard.Invoke(_cardScript);
        transform.DOScale(Vector3.one, 0.2f);
        _selected = false;
    }

    public void Select()
    {
        _onSelectCard.Invoke(_cardScript);
        transform.DOScale(Vector3.one * 1.1f, 0.2f);
        _selected = true;
    }

    private void OnDestroy()
    {
        _onSelectCard.RemoveAllListeners();
    }

    internal void SetCard(Card card)
    {
        _cardScript.SetCard(card);
    }

}
[Serializable]
public class OnSelectCard : UnityEvent<CardScript> { }
[Serializable]
public class OnDeselectCard : UnityEvent<CardScript> { }