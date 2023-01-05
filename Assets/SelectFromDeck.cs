using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFromDeck : MonoBehaviour
{
    [SerializeField]
    private DeckDisplay _deckDisplay;
    public OnSelectCard onSelectCard;
    private void Awake()
    {
        onSelectCard ??= new OnSelectCard();
    }

    private void OnDestroy()
    {
        onSelectCard?.RemoveAllListeners();
    }
    public void ChooseFromDeck(List<Card> deck)
    {
        _deckDisplay.gameObject.SetActive(true);
        _deckDisplay.SetCards(deck);
        var cardSelectors = _deckDisplay.GetComponentsInChildren<CardSelector>();
        foreach(var card in cardSelectors)
        {
            card.OnSelectCard.AddListener(SelectCard);
        }
    }

    public void SelectCard(CardScript cardSelector)
    {
        _deckDisplay.gameObject.SetActive(false);
        onSelectCard.Invoke(cardSelector);
    }
}
