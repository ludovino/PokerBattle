using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using URandom = UnityEngine.Random;

[CreateAssetMenu(menuName = "Level/BattleEncounterType")]
public class BattleEncounterType : EncounterType, IOnInit
{
    [SerializeField]
    private List<EnemyData> _enemyList;
    private List<EnemyData> _sessionPool;
    private Queue<EnemyData> _toPlay;
    [SerializeField]
    private List<RewardChance> _rewardChances; 
    private void OnEnable()
    {
        Init();
    }

    public void Requeue()
    {
        _toPlay.Clear();
        _sessionPool.Shuffle();
        _sessionPool.ForEach(e => _toPlay.Enqueue(e));
    }

    public void OnBeginBattle(EnemyData enemyData)
    {
        _sessionPool.Remove(enemyData);
        _sessionPool.ForEach(e => e.LevelUp());
        Requeue();
    }

    private List<RewardGenerator> GetRewards()
    {
        return _rewardChances.Where(c => c.chance >= URandom.value).Select(c => c.rewardGenerator).ToList();
    }

    public override Encounter GetEncounter() => _toPlay.Count > 0 ? new BattleEncounter(_toPlay.Dequeue(), this, GetRewards()) : null;

    public void Init()
    {
        _sessionPool = _enemyList.ToList();
        _sessionPool.Shuffle();
        _toPlay = new Queue<EnemyData>(_sessionPool);
    }
    [Serializable]
    private class RewardChance
    {
        [SerializeField]
        public RewardGenerator rewardGenerator;
        [SerializeField]
        public float chance;
    }
}


public class BattleEncounter : Encounter
{
    private readonly EnemyData _enemyData;
    private readonly BattleEncounterType _encounterType;
    private readonly List<RewardGenerator> _rewardGenerators;
    public BattleEncounter(EnemyData enemyData, BattleEncounterType encounterType, List<RewardGenerator> rewardGenerators)
    {
        _enemyData = enemyData;
        _encounterType = encounterType;
        _rewardGenerators = rewardGenerators;
    }
    public override string Name => _enemyData.name;

    public override string Tooltip => 
        $"~{_enemyData.displayName}\n" +
        $"@chips: {_enemyData.Chips}\n" +
        $"@blind: {GameController.GetBlind()}\n";

    public override EncounterType EncounterType => _encounterType;
    public override void BeginEncounter()
    {
        GameController.Instance.BeginBattle(_enemyData, _rewardGenerators);
        _encounterType.OnBeginBattle(_enemyData);
    }

    public override void SkipEncounter()
    {
        _encounterType.Requeue();
    }
}

