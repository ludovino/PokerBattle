using UnityEngine;
[CreateAssetMenu(menuName = "Chip")]
public class Chip : ScriptableObject
{
    [SerializeField]
    private int _value;
    [SerializeField]
    private Sprite _sprite;

    public int Value => _value;
    public Sprite Sprite => _sprite;
}
