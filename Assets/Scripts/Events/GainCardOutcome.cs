using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Event/Outcome/GainCard")]
public class GainCardOutcome : Outcome, ISingularOutcome
{
    [SerializeField]
    private string _card;

    public override void Execute()
    {
        PlayerData.Instance.AddCard(CardFactory.Instance.GetCard(_card));
    }

    public override void Execute(Action onComplete)
    {
        PlayerData.Instance.AddCard(CardFactory.Instance.GetCard(_card));
        onComplete();
    }
}