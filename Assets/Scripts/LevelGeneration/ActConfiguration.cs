using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using URandom = UnityEngine.Random;
[CreateAssetMenu(menuName = "Level/Act")]
public class ActConfiguration : ScriptableObject
{
    public List<LevelConfiguration> levels;
    public int level;
    public int pointsForWinning;
    public bool IsBossLevel => levels[level].isBoss;
    
    [SerializeField]
    private string _displayName; 
    public string DisplayName => _displayName;

    [SerializeField]
    private int _score;
    public int Score => _score;

    public List<Encounter> GetEncounters()
    {
        return levels[level].GetEncounters();
    }
    public LevelConfiguration GetLevel()
    {
        return levels[level];
    }
}

[Serializable]
public class LevelConfiguration
{
    public List<TableConfiguration> tables;
    public int blind;
    public bool isBoss;
    internal List<Encounter> GetEncounters()
    {
        return tables.Select(t => URandom.value <= t.chance ? t.type.GetEncounter() : null).Where(e => e != null).ToList();
    }
}

[Serializable]
public class TableConfiguration
{
    public EncounterType type;
    public float chance;
}