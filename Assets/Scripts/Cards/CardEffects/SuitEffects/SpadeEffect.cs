using UnityEngine;

public class SpadeEffect : CardEffect, IOnPlay
{
    public int damage;
    public override void Execute(PlayContext context)
    {
        var enemyCard = context.Opponent.fieldOfPlay[context.PlayIndex];
        if (enemyCard is null) return;
        DoEffect(context);
        enemyCard.ChangeValue(-damage);
    }
}

