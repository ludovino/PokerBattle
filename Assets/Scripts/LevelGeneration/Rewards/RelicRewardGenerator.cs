using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
[CreateAssetMenu(menuName = "Rewards/RelicGenerator")]
public class RelicRewardGenerator : RewardGenerator
{
    [SerializeField]
    private List<Relic> relics;
    private void Awake()
    {
        relics ??= new List<Relic>();
    }
    public override IReward Generate()
    {
        var relic = GetRelic();
        if (relic == null) return null;
        return new RelicReward(relic);
    }

    private Relic GetRelic()
    {
        var pool = relics.Except(PlayerData.Instance.Relics).ToList();
        pool.Shuffle();
        return pool.FirstOrDefault();
    }

    public class RelicReward : IReward
    {
        private UnityEvent _completed;
        private Relic _relic;
        private bool _complete;

        public RelicReward(Relic relic)
        {
            _completed ??= new UnityEvent();
            _relic = relic;
            _complete = false;
        }

        public UnityEvent Completed => _completed;

        public Sprite Sprite => _relic.Sprite;

        public string Name => _relic.DisplayName;

        public string Tooltip => _relic.Description;

        public bool Complete => _complete;

        public void OpenReward()
        {
            PlayerData.Instance.AddRelic(_relic);
            _complete = true;
            Completed.Invoke();
            Completed.RemoveAllListeners();
        }
    }
}

