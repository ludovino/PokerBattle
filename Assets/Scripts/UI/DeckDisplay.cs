using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
    [SerializeField]
    private CardScript _cardPrefab;
    private List<Card> _cards;
    [SerializeField]
    private Transform _grid;
    [SerializeField]
    private EntityData _entityData;

    public void SetCards(IEnumerable<Card> cards)
    {
        if (_cards != null && cards.SequenceEqual(_cards)) return;
        _cards = cards.ToList();
        _grid.DestroyChildren();
        foreach(var card in cards)
        {
            var cardDisplay = Instantiate(_cardPrefab, _grid);
            cardDisplay.SetCard(card, _entityData);
        }
    }
}
