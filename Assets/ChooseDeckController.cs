using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChooseDeckController : MonoBehaviour
{
    [SerializeField]
    private List<Suit> _selectedSuits;

    [SerializeField]
    private SuitToggle _togglePrefab;

    [SerializeField]
    private RectTransform _toggleParent;

    [SerializeField]
    private CardPool _playerCardPool;
    [SerializeField]
    private SuitList _playerSuitList;
    [SerializeField]
    private EntityData _player;

    [SerializeField]
    private CardScript _cardPrefab;
    
    [SerializeField]
    private SuitDisplay[] _suitDisplays = new SuitDisplay[4];

    private List<SuitToggle> _suitToggles;
    private List<GameObject> _cardObjects = new List<GameObject>();
    [SerializeField]
    private CardCollection _cardCollection; 
    [SerializeField]
    private Transform _drawOrigin;
    private List<Card> _cards;

    private void Start()
    {
        _suitToggles = new List<SuitToggle>();
        foreach(var suit in MetaProgress.Instance.UnlockedSuits)
        {
            if(suit == null) continue;
            var toggle = Instantiate(_togglePrefab, _toggleParent);
            toggle.Init(_selectedSuits.Contains(suit), suit, UpdateSuitsList);
            _suitToggles.Add(toggle);
        }
        DisplaySuits();
        _playerSuitList.SetSuits(_selectedSuits, 5);
    }
    private void AddSuit(Suit suit)
    {
        _selectedSuits.Add(suit);
        if (_selectedSuits.Count > 4) RemoveSuit(_selectedSuits[0]);
    }

    private void RemoveSuit(Suit suit)
    {
        _selectedSuits.Remove(suit);
        _suitToggles.First(t => suit == t.Suit).Select(false);
    }

    private void UpdateSuitsList(SuitToggle toggle)
    {
        if (_selectedSuits.Contains(toggle.Suit) == toggle.Selected) return;
        if (toggle.Selected) AddSuit(toggle.Suit);
        else RemoveSuit(toggle.Suit);
        
        DisplaySuits();
        _playerSuitList.SetSuits(_selectedSuits, 5);
    }

    private void DisplaySuits()
    {
        for(var i = 0; i < 4; i++)
        {
            var suit = _selectedSuits.ElementAtOrDefault(i);
            _suitDisplays[i].SetDisplay(suit);
        }
    }

    public void DrawCards()
    {
        _cards = _playerCardPool.GetWithoutReplacement(20, false);
        _cards.Sort((x, y) => x.highCardRank.CompareTo(y.highCardRank));
        _player.SetDeck(_cards);
        foreach(var card in _cards)
        {
            var cardObj = Instantiate(_cardPrefab, _drawOrigin.position, Quaternion.identity);
            cardObj.SetCard(card, _player);
            _cardObjects.Add(cardObj.gameObject);
        }

        _cardCollection.Clear();
        CoroutineQueue.Defer(_cardCollection.AddCards(_cardObjects));
    }

    public void Go()
    {
        _player.SetDeck(_cards);
        GameController.Instance.DeckChosen();
    }
}
