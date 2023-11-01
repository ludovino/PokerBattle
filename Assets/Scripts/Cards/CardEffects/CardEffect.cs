
using UnityEngine;

public abstract class CardEffect : ScriptableObject, ICardEffect
{
    [SerializeField]
    private string _description;
    public string Description => _description;
    [SerializeField]
    private GameObject _effectDisplay;

    protected void DoEffect(CardEffectContext context)
    {
        if (_effectDisplay is null) return;
        var display = Instantiate(_effectDisplay, context.Card.transform.position, context.Card.transform.rotation);
    }
    public abstract bool Condition(ICard card);
    public abstract void Trigger(CardEffectContext context);
    public virtual void OnEffectAdd(CardEffectContext context) { }
    public virtual void OnEffectRemove(CardEffectContext context) { }
}

