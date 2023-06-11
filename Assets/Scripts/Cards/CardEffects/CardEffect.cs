
using UnityEngine;

public abstract class CardEffect : ScriptableObject, ICardEffect
{
    [SerializeField]
    private string _description;
    public string Description => _description;
    [SerializeField]
    private CardEffectDisplay _effectDisplay;
    [SerializeField]
    private AudioClip _soundEffect;

    protected void DoEffect(CardEffectContext context)
    {
        if (_effectDisplay is null) return;
        var display = Instantiate(_effectDisplay, context.Card.transform.position, context.Card.transform.rotation);
        display.Init(context, _soundEffect);
    }
    public abstract bool Condition(ICard card);

    public abstract void Trigger(CardEffectContext context);
}

