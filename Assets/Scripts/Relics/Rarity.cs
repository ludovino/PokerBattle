using UnityEngine;

public class Rarity : ScriptableObject
{
    [SerializeField]
    private string _displayName;
    public string DisplayName => _displayName;

    [SerializeField]
    private float _chance;
    public float Chance => _chance;
}

