using UnityEngine;

public class PoisonEffect : CardEffect, IOnOpponentTurn
{
    [SerializeField]
    private int damage;
    public override void Execute(PlayContext context)
    {
        var enemyCard = context.Opponent.fieldOfPlay[context.PlayIndex];
        if (enemyCard is null) return;
        DoEffect(context);
        enemyCard.ChangeValue(-damage);
    }
}