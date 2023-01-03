
public class LifeEffect : SuitEffect, IOnPlay
{
    public override void Execute(CardEffectContext context)
    {
        context.Battle.TakeFromPot(context.Owner, context.Battle.StartingBlind);
    }
}
