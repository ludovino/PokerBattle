using UnityEngine;
[CreateAssetMenu(menuName = "Card/Face")]
public class Face : ScriptableObject
{
    public string numeral;
    public string longName;
    public int highCardRank;
    public int blackjackValue;
    public Face higher;
    public Face lower;
    public Sprite blankSprite;
    public AnimationClip clip;
    public int price;
}