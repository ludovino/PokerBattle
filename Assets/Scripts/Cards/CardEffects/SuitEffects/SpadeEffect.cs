using UnityEngine;

public class SpadeEffect : SuitEffect, IOnPlay
{
    public int damage;
    public override void Execute(CardEffectContext context)
    {
        var enemyCard = context.Opponent.fieldOfPlay[context.PlayIndex];
        if (enemyCard is null) return;
        DoEffect(context);
        enemyCard.ChangeValue(-damage);
    }
}

