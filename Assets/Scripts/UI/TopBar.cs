using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    [SerializeField]
    private EntityData _player;
    [SerializeField]
    private TextMeshProUGUI _chipCount;
    [SerializeField]
    private TextMeshProUGUI _deckCount;
    [SerializeField]
    private DeckDisplay _deckDisplay;
    [SerializeField]
    private HandListDisplay _handListDisplay;

    private void Start()
    {
        _player.OnChangeChips.AddListener(UpdateChips);
        _player.OnChangeDeck.AddListener(UpdateDeck);
        UpdateChips(0, _player.Chips, 0);
        UpdateDeck(_player.Cards.OrderBy(c => c.highCardRank).ToList());

    }

    private void UpdateChips(int _, int current, int __)
    {
        CoroutineQueue.Defer(CR_UpdateChips(current));
    }
    private IEnumerator CR_UpdateChips(int current)
    {
        _chipCount.text = current.ToString();
        yield return null;
    }
    private void UpdateDeck(IReadOnlyCollection<Card> cards)
    {
        _deckDisplay.SetCards(cards);
        _deckCount.text = cards.Count.ToString();
    }
    private void OnDestroy()
    {
        _player.OnChangeChips.RemoveListener(UpdateChips);
        _player.OnChangeDeck.RemoveListener(UpdateDeck);
    }

    public void ToggleDeckView()
    {
        _deckDisplay.gameObject.SetActive(!_deckDisplay.gameObject.activeInHierarchy);
    }
    public void ToggleHandView()
    {
        _handListDisplay.gameObject.SetActive(!_handListDisplay.gameObject.activeInHierarchy);
    }
}
