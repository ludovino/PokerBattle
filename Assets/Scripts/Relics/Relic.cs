using UnityEngine;

public abstract class Relic : ScriptableObject
{
    [SerializeField]
    private string _displayName;
    public string DisplayName => _displayName;
    [SerializeField]
    private string _description;
    public string Description => _description;
    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite => _sprite;
}