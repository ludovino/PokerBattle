using UnityEngine;

public class PoisonEffect : SuitEffect, IOnOpponentTurn
{
    [SerializeField]
    private int damage;
    public override void Execute(CardEffectContext context)
    {
        var enemyCard = context.Opponent.fieldOfPlay[context.PlayIndex];
        if (enemyCard is null) return;
        DoEffect(context);
        enemyCard.ChangeValue(-damage);
    }
}