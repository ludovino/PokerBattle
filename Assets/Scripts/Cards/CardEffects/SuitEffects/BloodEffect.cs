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
        DoEffect(context);
        context.OpposingCard.ChangeValue(-damage);
        context.Card.ChangeValue(damage);
    }
}
