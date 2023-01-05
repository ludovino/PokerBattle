using UnityEngine;

[CreateAssetMenu(menuName = "Event/Outcome/RemoveCard")]
public class RemoveCardOutcome : Outcome
{
    [SerializeField]
    private SelectFromDeck _selectFromDeckPrefab;
    public override void Execute()
    {
        var cardList = PlayerData.Instance.CloneDeck;
        var menu = Instantiate(_selectFromDeckPrefab);
        menu.ChooseFromDeck(cardList);
        menu.onSelectCard.AddListener(OnSelect);
    }

    private void OnSelect(CardScript cardScript)
    {
        PlayerData.Instance.RemoveCard(cardScript.card);
    }
}

