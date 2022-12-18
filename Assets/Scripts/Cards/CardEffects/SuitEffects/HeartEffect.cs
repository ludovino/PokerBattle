public class HeartEffect : CardEffect, IOnPlayerTurn
{
    public override void Execute(PlayContext context)
    {
        if (context.Card.valueDifference >= 0) return;
        DoEffect(context);
        context.Card.ResetCard(animate: true);
    }
}