internal class BloodEffect : SuitEffect, IOnPlay
{
    public int damage;

    public void OnPlay(CardEffectContext context)
    {
        
        if (context.OpposingCard is null) return;
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        if (context.OpposingCard.highCardRank == 0) return;
        var sprite = context.OpposingCard.ChangeValue(-damage);
        var selfSprite = context.Card.ChangeValue(damage);
        CoroutineQueue.Defer(() => {
            context.OpposingCard.SetSprite(sprite);
            context.Card.SetSprite(selfSprite); 
        });
    }
}
