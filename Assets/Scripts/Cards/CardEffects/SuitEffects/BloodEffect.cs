internal class BloodEffect : SuitEffect, IOnPlay
{
    public int damage;

    public override void Trigger(CardEffectContext context)
    {
        var enemyCard = context.Opponent.fieldOfPlay[context.PlayIndex];
        if (enemyCard is null || enemyCard.highCardRank == 0) return;
        DoEffect(context);
        enemyCard.ChangeValue(-damage);
        context.Card.ChangeValue(damage);
    }
}
