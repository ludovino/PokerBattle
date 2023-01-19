using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using URandom = UnityEngine.Random;

[CreateAssetMenu(menuName = "Relic/Pool")]
public class RelicPool : ScriptableObject
{
    [SerializeField]
    private List<Relic> _poolRelics;
    private List<Relic> _pool;
    public IReadOnlyList<Relic> Relics => _pool;
    private List<RarityPool> _rarityPools;
    public void UpdateRelics()
    {
        _pool.Clear();
        _pool.AddRange(MetaProgress.Instance.UnlockedRelics.Where(r => _poolRelics.Contains(r)));
        _pool.RemoveAll(r => !r.CanFind);
        _rarityPools.Clear();
        _rarityPools.AddRange(
            _pool
            .GroupBy(r => r.Rarity)
            .Select(g => 
            new RarityPool() {
                Pool = g.ToList(),
                Rarity = g.Key
            }));
    }

    public Relic GetRelic()
    {
        var rarity = SelectRelicRarity();
        return rarity[URandom.Range(0, rarity.Count)];
    }

    private List<Relic> SelectRelicRarity()
    {
        var total = _rarityPools.Sum(c => c.Rarity.Chance);
        var x = URandom.Range(0, total);
        foreach (var pool in _rarityPools)
        {
            x -= pool.Rarity.Chance;
            if (x <= 0)
            {
                return pool.Pool;
            }
        }
        return _rarityPools.Last().Pool;
    }

    private class RarityPool
    {
        public Rarity Rarity;
        public List<Relic> Pool;
    }
}
