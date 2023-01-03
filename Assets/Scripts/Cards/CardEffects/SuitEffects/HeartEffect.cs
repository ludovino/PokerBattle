public class HeartEffect : SuitEffect, IOnPlayerTurn
{
    public override void Execute(CardEffectContext context)
    {
        if (context.Card.valueDifference >= 0) return;
        DoEffect(context);
        context.Card.ResetCard(animate: true);
    }
}