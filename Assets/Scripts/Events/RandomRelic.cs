using UnityEngine;

[CreateAssetMenu(menuName = "Event/Outcome/RandomRelic")]

public class RandomRelic : Outcome
{
    [SerializeField]
    private RelicPool _relicPool;
    public override void Execute()
    {
        var relic = _relicPool.GetRelic();
        PlayerData.Instance.AddRelic(relic);
    }
}

