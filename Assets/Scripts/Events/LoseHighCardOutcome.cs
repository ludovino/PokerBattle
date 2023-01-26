using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Outcome/LoseHighCard")]
public class LoseHighCardOutcome : Outcome
{
    public override void Execute()
    {
        var card = PlayerData.Instance.Cards.OrderByDescending(c => c.highCardRank).First();
        PlayerData.Instance.RemoveCard(card);
    }

    public override void Execute(Action onComplete)
    {
        var card = PlayerData.Instance.Cards.OrderByDescending(c => c.highCardRank).First();
        PlayerData.Instance.RemoveCard(card);
        onComplete();
    }
}

