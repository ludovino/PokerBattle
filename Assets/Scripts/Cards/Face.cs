using UnityEngine;
[CreateAssetMenu(menuName = "Face")]
public class Face : ScriptableObject
{
    public string numeral;
    public string longName;
    public int highCardRank;
    public int blackjackValue;
    public Sprite blankSprite;
    public AnimationClip clip;
}