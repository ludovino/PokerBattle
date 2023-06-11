
public class LifeEffect : SuitEffect, IOnPlay
{
    public override void Trigger(CardEffectContext context)
    {
        context.Battle.TakeFromPot(context.Owner, context.Battle.StartingBlind);
    }
}
