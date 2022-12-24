using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
    [SerializeField]
    private CardScript _cardPrefab;
    private List<Card> _cards;
    [SerializeField]
    private GameObjectGrid _grid;

    public void SetCards(IEnumerable<Card> cards)
    {
        if (_cards != null && cards.SequenceEqual(_cards)) return;
        _cards = cards.ToList();
        _grid.Clear();
        foreach(var card in cards)
        {
            var cardDisplay = Instantiate(_cardPrefab);
            cardDisplay.SetCard(card);
            _grid.Add(cardDisplay.gameObject);
        }
    }
}
