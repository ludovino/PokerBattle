using UnityEngine;
[CreateAssetMenu(menuName = "Unlock/Suit")]
internal class UnlockableSuit : Unlockable
{
    [SerializeField]
    private Suit _suit;
    public Suit Suit => _suit;

    public override UnlockDisplay GetDisplay()
    {
        return new UnlockDisplay() { Name = _suit.longName, Sprite = _suit.displaySprite, Description = PlayerData.Instance.EffectList.SuitEffectDescription(_suit) };
    }
}