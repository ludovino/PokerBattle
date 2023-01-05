using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Outcome/Multiple")]
public class MultipleOutcome : Outcome, IMultipleOutcome
{
    [SerializeField]
    private List<Outcome> _outcomes;
    public override void Execute()
    {
        foreach(var outcome in _outcomes)
        {
            outcome.Execute();
        }
    }

    private void OnValidate()
    {
        if (_outcomes.OfType<IMultipleOutcome>().Any()) throw new Exception("Nested Multiple Outcomes");
    }

    public override string Description => GetDescription();
    private string GetDescription()
    {
        if (_outcomes.Count == 2)
        {
            return string.Join(", ", _outcomes.Select(oc => oc.Description));
        }
        return base.Description;
    }
}
