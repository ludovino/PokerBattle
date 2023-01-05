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
    public bool IsBossLevel => levels[level].isBoss;

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