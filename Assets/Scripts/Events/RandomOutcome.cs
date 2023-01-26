using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using URandom = UnityEngine.Random;


[CreateAssetMenu(menuName = "Event/Outcome/Random")]
public class RandomOutcome : Outcome, IMultipleOutcome
{
    [SerializeField]
    List<OutcomeChance> _outcomeChances;
    
    public override void Execute()
    {
        
    }

    public override string Description => GetDescription();
    private string GetDescription()
    {
        if(string.IsNullOrEmpty(base.Description) &&_outcomeChances.Count == 2)
        {
            return string.Join(", ", _outcomeChances.Select(oc => oc.Description));
        }
        return base.Description;
    }

    public override void Execute(Action onComplete)
    {
        var total = _outcomeChances.Sum(c => c.chance);
        var x = URandom.Range(0, total);
        foreach (var outcome in _outcomeChances)
        {
            x -= outcome.chance;
            if (x <= 0)
            {
                outcome.outcome.Execute(onComplete);
                return;
            }
        }
    }

    [Serializable]
    public class OutcomeChance
    {
        public Outcome outcome;
        public float chance;
        public bool showChance;
        public string Description => showChance ? $"{chance * 100}% {outcome.Description}" : outcome.Description;
    }
}
