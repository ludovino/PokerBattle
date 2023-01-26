using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Outcome/ChooseCard")]
public class ChooseCardOutcome : Outcome, ISingularOutcome
{
    [SerializeField]
    private CardPool _cardPool;
    [SerializeField]
    private int _count;
    [SerializeField]
    private CardSelectMenu _cardSelectPrefab;

    public override void Execute()
    {
        throw new NotImplementedException();
    }

    public override void Execute(Action onComplete)
    {
        var menu = Instantiate(_cardSelectPrefab);
        menu.Init(_cardPool.GetWithReplacement(_count), 1);
        menu.StartSelect();
        menu.OnSelect.AddListener(OnSelect);
        menu.OnSelect.AddListener(_ => onComplete());
    }

    private void OnSelect(List<CardScript> cards)
    {
        foreach (var card in cards)
        {
            PlayerData.Instance.AddCard(card.card);
        }
    }
}
