
public class LifeEffect : SuitEffect, IOnPlay
{
    public void OnPlay(CardEffectContext context)
    {
        throw new System.NotImplementedException();
    }

    public override void Trigger(CardEffectContext context)
    {
        context.Battle.TakeFromPot(context.Owner, context.Battle.StartingBlind);
    }
}
