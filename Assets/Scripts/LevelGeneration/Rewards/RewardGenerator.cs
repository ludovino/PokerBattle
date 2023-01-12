using UnityEngine;
using UnityEngine.Events;

public abstract class RewardGenerator : ScriptableObject
{
    public abstract IReward Generate();
}

public interface IReward
{
    public UnityEvent Completed { get; }
    public bool Complete { get; }
    public Sprite Sprite { get; }
    public string Name { get; }
    public string Tooltip { get; }
    public void OpenReward();
}