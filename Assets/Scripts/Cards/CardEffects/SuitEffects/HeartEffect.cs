public class HeartEffect : SuitEffect, IOnPlayerTurn
{
    public void OnPlayerTurn(CardEffectContext context)
    {
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        if (context.Card.valueDifference >= 0) return;
        var sprite = context.Card.ResetCardAnimated();
        CoroutineQueue.Defer(() => context.Card.SetSprite(sprite));
    }
}