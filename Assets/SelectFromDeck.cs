using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectFromDeck : MonoBehaviour
{
    [SerializeField]
    private DeckDisplay _deckDisplay;
    public OnSelectCard onSelectCard;
    public UnityEvent onExit;
    public Button exitButton;
    private void Awake()
    {
        onSelectCard ??= new OnSelectCard();
        onExit ??= new UnityEvent();
    }

    private void OnDestroy()
    {
        onSelectCard?.RemoveAllListeners();
    }
    public void ChooseFromDeck(List<Card> deck, bool skippable = true)
    {
        CoroutineQueue.Defer(CR_Choose(deck));
        if (!skippable) exitButton.gameObject.SetActive(false);
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

    public void Exit()
    {
        onExit.Invoke();
    }
}
