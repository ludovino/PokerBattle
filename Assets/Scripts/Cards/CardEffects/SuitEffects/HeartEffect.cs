public class HeartEffect : SuitEffect, IOnPlayerTurn
{
    public void OnPlayerTurn(CardEffectContext context)
    {
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        if (context.Card.valueDifference >= 0) return;
        DoEffect(context);
        context.Card.ResetCard(animate: true);
    }
}