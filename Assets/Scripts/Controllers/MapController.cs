using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private ActConfiguration _act;
    public List<EncounterDisplay> encounterDisplays;
    public EncounterDisplay bossEncounter;
    public UnityEvent OnBoss;
    private List<Encounter> _encounters;
    public void Awake()
    {
        OnBoss = OnBoss ?? new UnityEvent();
        _encounters = new List<Encounter>();
    }
    public void Start()
    {
        var act = GameController.Instance?.currentAct ?? _act;
        if (act.IsBossLevel)
        {
            OnBoss.Invoke();
            bossEncounter.Init(act.GetEncounters().First());
            return;
        }
        _encounters = act.GetEncounters();
        
        AssignTables(_encounters);
    }

    private void AssignTables(List<Encounter> encounters)
    {
        var indicies = encounterDisplays.Select((_, i) => i).ToList();
        indicies.Shuffle();
        for(int i = 0; i < encounterDisplays.Count; i++)
        {
            encounterDisplays[indicies[i]].Init(encounters.ElementAtOrDefault(i));
        }
    }

    public void OnBeginEncounter(Encounter encounter)
    {
        _encounters.Where(e => e != encounter).ToList().ForEach(e => e.SkipEncounter());
    }
}

