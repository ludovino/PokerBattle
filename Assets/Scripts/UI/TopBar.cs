using System.Collections.Generic;
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
        _player.ChangeChips(0);
        _player.TriggerDeckChange();

    }

    private void UpdateChips(int _, int current, int __)
    {
        _chipCount.text = current.ToString();
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
