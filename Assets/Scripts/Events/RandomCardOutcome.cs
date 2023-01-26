using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Outcome/RandomCard")]
public class RandomCardOutcome : Outcome, ISingularOutcome
{
    [SerializeField]
    private CardPool _cardPool;
    [SerializeField]
    private int _setCount;
    public override void Execute()
    {
        var card = _cardPool.GetOne();
        
        if (_setCount <= 1)
        {
            PlayerData.Instance.AddCard(card);
            return;
        }

        for (var i = 0; i < _setCount; i++)
        {
            PlayerData.Instance.AddCard(card);
        }
    }

    public override void Execute(Action onComplete)
    {
        Execute();
        onComplete();
    }
}
