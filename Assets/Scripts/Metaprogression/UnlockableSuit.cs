using UnityEngine;
[CreateAssetMenu(menuName = "Unlock/Suit")]
internal class UnlockableSuit : Unlockable
{
    [SerializeField]
    private Suit _suit;
    public Suit Suit => _suit;

    protected override UnlockDisplay DoUnlock()
    {
        return new UnlockDisplay() { Name = _suit.longName, Sprite = _suit.sprite, Description = PlayerData.Instance.EffectList.SuitEffectDescription(_suit) };
    }
}