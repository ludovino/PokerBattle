using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Event/Outcome/ChangeChips")]
public class ChangeChipsOutcome : Outcome, ISingularOutcome
{
    [SerializeField]
    private int _amount;

    public override void Execute()
    {
        PlayerData.Instance.ChangeChips(_amount);
    }

    public override void Execute(Action onComplete)
    {
        Execute();
        onComplete();
    }
}
