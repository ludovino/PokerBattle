
using UnityEngine;

public abstract class CardEffect : ScriptableObject, ICardEffect
{
    [SerializeField]
    private string _description;
    public string Description => _description;
    public abstract bool Condition(ICard card);
    public abstract void Trigger(CardEffectContext context);
    public virtual void OnEffectAdd(CardEffectContext context) { }
    public virtual void OnEffectRemove(CardEffectContext context) { }
}

