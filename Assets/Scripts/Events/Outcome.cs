using System;
using UnityEngine;

public abstract class Outcome : ScriptableObject
{
    [SerializeField]
    private string _description;
    public virtual string Description => _description;
    [SerializeField]
    private Sentiment _value;
    public Sentiment Value => _value;
    public abstract void Execute();
    public abstract void Execute(Action onComplete);

    public enum Sentiment
    {
        Positive,
        Negative,
        Neutral
    }
}

public interface ISingularOutcome { }
public interface IMultipleOutcome { }