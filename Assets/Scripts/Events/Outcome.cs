using UnityEngine;

public abstract class Outcome : ScriptableObject
{
    [SerializeField]
    private string _description;
    public virtual string Description => GetPrefix() + _description;
    [SerializeField]
    private Sentiment _value;
    public Sentiment Value => _value;
    public abstract void Execute();
    protected string GetPrefix()
    {
        return _value switch
        {
            Sentiment.Positive => "++",
            Sentiment.Negative => "--",
            Sentiment.Neutral => "+-",
            _ => string.Empty,
        };
    }
    public enum Sentiment
    {
        Positive,
        Negative,
        Neutral
    }
}

public interface ISingularOutcome { }
public interface IMultipleOutcome { }