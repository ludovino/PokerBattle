using UnityEngine;

[CreateAssetMenu(menuName = "Unlock/Relic")]
public class UnlockableRelic : Unlockable
{
    [SerializeField]
    private Relic _relic;
    public Relic Relic => _relic;

    public override UnlockDisplay GetDisplay()
    {
        return new UnlockDisplay() { Name = _relic.DisplayName, Description = _relic.Description, Sprite = _relic.Sprite };
    }
}

