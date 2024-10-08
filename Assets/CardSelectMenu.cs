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
    private Transform _grid;
    [SerializeField]
    private OnSelectCards _onSelect;
    [SerializeField]
    private EntityData _owner;

    private List<CardSelector> _cardSelectors;
    [SerializeField]
    private List<CardScript> _selected;
    private int _toSelect;
    private List<Card> _cards;
    private bool _chosen;

    private int _selectionsRemaining => _toSelect - _selected.Count;
    public OnSelectCards OnSelect => _onSelect;

    private void Awake()
    {
        _onSelect = _onSelect ?? new OnSelectCards();
    }

    private void OnDestroy()
    {
        _onSelect?.RemoveAllListeners();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnSelectCard(CardScript selector)
    {
        if(_selectionsRemaining == 0)
        {
            _selected.First().GetComponent<CardSelector>().Deselect();
        }
        _selected.Add(selector);
    }

    private void OnDeselectCard(CardScript selector)
    {
        _selected.Remove(selector);
    }

    public void Confirm()
    {
        _onSelect.Invoke(_selected);
        
        if(_selected.Count >= _toSelect)
        {
            foreach (var cardSelector in _cardSelectors)
            {
                cardSelector.OnSelectCard.RemoveListener(OnSelectCard);
            }
            _onSelect.RemoveAllListeners();
            gameObject.SetActive(false);
            _chosen = true;
        }
    }
    public void Init(List<Card> cards, int count)
    {
        _grid.DestroyChildren();
        _selected = new List<CardScript>();
        _cardSelectors = new List<CardSelector>();
        _toSelect = Mathf.Min(count, cards.Count);
        _cards = cards;

        foreach (var card in _cards)
        {
            var cardSelector = Instantiate(_cardSelectionPrefab, _grid);
            cardSelector.OnSelectCard.AddListener(OnSelectCard);
            cardSelector.OnDeselectCard.AddListener(OnDeselectCard);
            cardSelector.SetCard(card, _owner);
            _cardSelectors.Add(cardSelector);
        }

    }
    private IEnumerator CR_Open()
    {
        _chosen = false;
        gameObject.SetActive(true);
        yield return new WaitUntil(() => _chosen);
        // animate card selection
    }

    public void StartSelect()
    {
        CoroutineQueue.Defer(CR_Open());
    }
}

public class OnSelectCards : UnityEvent<List<CardScript>> { }