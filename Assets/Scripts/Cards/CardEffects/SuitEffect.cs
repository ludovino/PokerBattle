using UnityEngine;

public abstract class SuitEffect : ScriptableObject, ICardEffect
{
    [SerializeField]
    private string _description;
    [SerializeField]
    private Suit _suit;
    public Suit Suit => _suit;
    public string Description => _description;
    [SerializeField]
    private CardEffectDisplay _effectDisplay;
    [SerializeField]
    private AudioClip _soundEffect;
    protected void DoEffect(CardEffectContext context)
    {
        if (_effectDisplay is null)  return;
        var display = Instantiate(_effectDisplay,context.Card.transform.position, context.Card.transform.rotation);
        display.Init(context, _soundEffect);
    }
    public abstract void Execute(CardEffectContext context);

    public bool Condition(ICard card)
    {
        return card.suit == Suit;
    }
}