using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RelicListDisplay : MonoBehaviour
{
    private List<Relic> _relics = new List<Relic>();
    [SerializeField]
    private RelicDisplay _relicDisplayPrefab;
    private void Start()
    {
        PlayerData.Instance.OnAddRelic.AddListener(AddRelic);
        AddRelic(PlayerData.Instance.Relics);
    }

    private void AddRelic(IReadOnlyList<Relic> list)
    {
        var toAdd = list.Except(_relics).ToList();
        _relics.AddRange(toAdd);
        CoroutineQueue.Defer(CR_AddRelicObject(toAdd));
    }
    public IEnumerator CR_AddRelicObject(List<Relic> relics)
    {
        foreach(Relic relic in relics)
        {
            var relicDisplay = Instantiate(_relicDisplayPrefab, transform);
            relicDisplay.Init(relic);
        }
        yield return null;
    }
}
