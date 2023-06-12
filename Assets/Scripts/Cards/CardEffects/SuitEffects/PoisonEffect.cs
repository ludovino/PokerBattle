using UnityEngine;

public class PoisonEffect : SuitEffect, IOnOpponentTurn
{
    [SerializeField]
    private int damage;

    public void OnOpponentTurn(CardEffectContext context)
    {
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        var enemyCard = context.OpposingCard;
        if (enemyCard is null) return;
        DoEffect(context);
        enemyCard.ChangeValue(-damage);
    }
}