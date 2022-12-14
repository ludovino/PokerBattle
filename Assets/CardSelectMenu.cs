using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CardSelectMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private CardSelector _cardSelectionPrefab;
    [SerializeField]
    private GameObjectGrid _grid;
    [SerializeField]
    private OnSelectCards _onSelect;

    private List<CardSelector> _cardSelectors;
    [SerializeField]
    private List<CardSelector> _selected;
    private int _toSelect;
    private List<Card> _cards;
    private bool _chosen;

    private int _selectionsRemaining => _toSelect - _selected.Count;
    public OnSelectCards OnSelect => _onSelect;

    private void Awake()
    {
        _onSelect = _onSelect ?? new OnSelectCards();
    }
    private void Start()
    {
        _menu.SetActive(false);
    }
    private void OnSelectCard(CardSelector selector)
    {
        if(_selectionsRemaining == 0)
        {
            _selected.First().Deselect();
        }
        _selected.Add(selector);
    }

    private void OnDeselectCard(CardSelector selector)
    {
        _selected.Remove(selector);
    }

    public void Confirm()
    {
        foreach (var cardSelector in _cardSelectors)
        {
            cardSelector.OnSelectCard.RemoveListener(OnSelectCard);
        }
        _onSelect.Invoke(_selected);
        _onSelect.RemoveAllListeners();
        _menu.SetActive(false);
        _chosen = true;
    }

    private IEnumerator CR_Open()
    {
        _chosen = false;
        _menu.SetActive(true);
        foreach (var card in _cards)
        {
            var cardSelector = Instantiate(_cardSelectionPrefab);
            cardSelector.OnSelectCard.AddListener(OnSelectCard);
            cardSelector.OnDeselectCard.AddListener(OnDeselectCard);
            cardSelector.SetCard(card);
            _grid.Add(cardSelector.gameObject);
            _cardSelectors.Add(cardSelector);
        }

        yield return new WaitUntil(() => _chosen);
    }

    public void StartSelect(List<Card> cards, int count)
    {
        _selected = new List<CardSelector>();
        _cardSelectors = new List<CardSelector>();
        _toSelect = Mathf.Min(count, cards.Count);
        _cards = cards;
        CoroutineQueue.Defer(CR_Open());
    }
}

public class OnSelectCards : UnityEvent<List<CardSelector>> { }