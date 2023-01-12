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
        CoroutineQueue.Defer(CR_Choose(deck));
    }

    public IEnumerator CR_Choose(List<Card> deck)
    {
        var _chosen = false;
        _deckDisplay.gameObject.SetActive(true);
        _deckDisplay.SetCards(deck);
        var cardSelectors = _deckDisplay.GetComponentsInChildren<CardSelector>();
        foreach (var card in cardSelectors)
        {
            card.OnSelectCard.AddListener(SelectCard);
            card.OnSelectCard.AddListener((_) => _chosen = true);
        }

        yield return new WaitUntil(() => _chosen);
    }

    public void SelectCard(CardScript cardSelector)
    {
        _deckDisplay.gameObject.SetActive(false);
        onSelectCard.Invoke(cardSelector);
    }
}
