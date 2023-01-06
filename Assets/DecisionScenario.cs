using TMPro;
using UnityEngine;

public class DecisionScenario : MonoBehaviour
{
    [SerializeField]
    private Scenario _scenario;

    [SerializeField]
    private OutcomeSelector _outcomeSelector;

    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _description;
    public void Start()
    {
        if (_scenario) Init(_scenario);
    }
    public void Init(Scenario scenario)
    {
        _scenario = scenario;
        _title.text = _scenario.Title;
        _description.text = _scenario.Description;
        _outcomeSelector.Init(scenario.Outcomes);
    }
}
