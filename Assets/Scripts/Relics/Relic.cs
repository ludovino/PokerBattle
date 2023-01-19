using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private Rarity _rarity;
    public Rarity Rarity => _rarity;
    [SerializeField]
    List<RelicCondition> _conditions;
    public void OnEnable()
    {
        _conditions ??= new List<RelicCondition>();
    }
    public bool CanFind => _conditions.Count == 0  || _conditions.All(c => c.Met());
}