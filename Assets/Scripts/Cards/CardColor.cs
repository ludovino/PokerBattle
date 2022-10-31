using UnityEngine;
[CreateAssetMenu(menuName = "CardColor")]
public class CardColor : ScriptableObject
{
    [SerializeField]
    private Color _color;
    public Color Value => _color;
}
