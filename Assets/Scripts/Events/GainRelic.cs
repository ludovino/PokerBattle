using UnityEngine;

[CreateAssetMenu(menuName = "Event/Outcome/GainRelic")]
public class GainRelic : Outcome
{
    [SerializeField]
    private Relic _relic;
    public override void Execute()
    {
        PlayerData.Instance.AddRelic(_relic);
    }
}
