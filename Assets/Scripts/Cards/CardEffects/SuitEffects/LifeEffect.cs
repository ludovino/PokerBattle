
public class LifeEffect : CardEffect, IOnPlay
{
    public override void Execute(PlayContext context)
    {
        context.Battle.TakeFromPot(context.Owner, context.Battle.StartingBlind);
    }
}
