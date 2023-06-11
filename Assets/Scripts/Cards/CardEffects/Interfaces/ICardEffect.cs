public interface ICardEffect
{
    public bool Condition(ICard card);
    public void Trigger(CardEffectContext context);
}