using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Outcome/RemoveCard")]
public class RemoveCardOutcome : Outcome
{
    [SerializeField]
    private SelectFromDeck _selectFromDeckPrefab;
    public override void Execute()
    {
        
    }

    public override void Execute(Action onComplete)
    {
        var cardList = PlayerData.Instance.CloneDeck;
        var menu = Instantiate(_selectFromDeckPrefab);
        menu.ChooseFromDeck(cardList, false);
        menu.onSelectCard.AddListener(OnSelect);
        menu.onSelectCard.AddListener(_ => onComplete());
        menu.onExit.AddListener(() => onComplete());
    }

    private void OnSelect(CardScript cardScript)
    {
        PlayerData.Instance.RemoveCard(cardScript.card);
    }
}

