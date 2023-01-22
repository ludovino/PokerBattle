using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/ScenarioEncounterType")]
public class ScenarioEncounterType : EncounterType, IOnInit
{
    [SerializeField]
    List<Scenario> _scenarios = new List<Scenario>();
    List<Scenario> _encounterPool = new List<Scenario>();

    [SerializeField]
    private string _tooltip;

    public override Encounter GetEncounter()
    {
        var encounter = _encounterPool.First();
        _encounterPool.Remove(encounter);
        _encounterPool.Add(encounter);
        return new ScenarioEncounter(encounter, this);
    }

    public void Init()
    {
        _encounterPool = _scenarios.ToList();
        _encounterPool.Shuffle();
    }

    public class ScenarioEncounter : Encounter
    {
        private Scenario _scenario;
        private ScenarioEncounterType _scenarioEncounterType;

        public ScenarioEncounter(Scenario scenario, ScenarioEncounterType scenarioEncounterType)
        {
            _scenario = scenario;
            _scenarioEncounterType = scenarioEncounterType;
        }

        public override string Name => _scenarioEncounterType.DisplayName;

        public override EncounterType EncounterType => _scenarioEncounterType;

        public override string Tooltip => _scenarioEncounterType._tooltip;

        public override void BeginEncounter()
        {
            _scenarioEncounterType._encounterPool.Remove(_scenario);
            _scenarioEncounterType._encounterPool.Shuffle();
            GameController.Instance.BeginScenario(_scenario);
        }

        public override void SkipEncounter()
        {
            _scenarioEncounterType._encounterPool.Shuffle();
        }
    }
}
