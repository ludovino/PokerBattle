public interface ICardEffect
{
    public bool Condition(ICard card);
    public void Execute(CardEffectContext context);
}