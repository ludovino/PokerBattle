﻿using UnityEngine;

public class PoisonEffect : SuitEffect, IOnOpponentTurn
{
    [SerializeField]
    private int damage;
    public override void Execute(CardEffectContext context)
    {
        var enemyCard = context.OpposingCard;
        if (enemyCard is null) return;
        DoEffect(context);
        enemyCard.ChangeValue(-damage);
    }
}