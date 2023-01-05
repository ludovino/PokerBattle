using UnityEngine;
[CreateAssetMenu(menuName = "Card/Color")]
public class CardColor : ScriptableObject
{
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Color _light;
    [SerializeField]
    private Color _dark;
    public Color Value => _color;
    public Color Light => _light;
    public Color Dark => _dark;

}
