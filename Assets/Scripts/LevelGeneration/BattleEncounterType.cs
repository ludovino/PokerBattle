using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleEncounterType")]
public class BattleEncounterType : EncounterType, IOnInit
{
    [SerializeField]
    private List<EnemyData> _enemyList;
    private List<EnemyData> _sessionPool;
    private Queue<EnemyData> _toPlay;

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
    public override Encounter GetEncounter() => _toPlay.Count > 0 ? new BattleEncounter(_toPlay.Dequeue(), this) : null;

    public void Init()
    {
        _sessionPool = _enemyList.ToList();
        _sessionPool.Shuffle();
        _toPlay = new Queue<EnemyData>(_sessionPool);
    }
}


public class BattleEncounter : Encounter
{
    private readonly EnemyData _enemyData;
    private readonly BattleEncounterType _encounterType;
    public BattleEncounter(EnemyData enemyData, BattleEncounterType encounterType)
    {
        _enemyData = enemyData;
        _encounterType = encounterType;
    }
    public override string Name => _enemyData.name;

    public override string Tooltip => 
        $"{_enemyData.displayName}\n" +
        $"chips: {_enemyData.Chips}\n" +
        $"blind: {GameController.GetBlind()}\n";

    public override EncounterType EncounterType => _encounterType;
    public override void BeginEncounter()
    {
        GameController.Instance.BeginBattle(_enemyData);
        _encounterType.OnBeginBattle(_enemyData);
    }

    public override void SkipEncounter()
    {
        _encounterType.Requeue();
    }
}

