using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    DecisionScenario _decisionScenario;
    public void Init(Scenario scenario)
    {
        _decisionScenario.Init(scenario);
    }

    public void BackToMap()
    {
        GameController.Instance.GoToNextLevel();
    }
}
