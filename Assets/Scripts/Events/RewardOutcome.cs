using System;
using System.Collections.Generic;
using UnityEngine;

internal class RewardOutcome : Outcome
{
    [SerializeField]
    RewardScreen _rewardScreenPrefab;
    [SerializeField]
    List<RewardGenerator> _rewardGenerator;
    public override void Execute()
    {
        throw new NotImplementedException();
    }
}

