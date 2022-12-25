using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidEffect : CardEffect, IAfterOpponentPlay
{
    public override void Execute(PlayContext context)
    {
        var card = context.Opponent.fieldOfPlay[context.PlayIndex];
        if (card.suit is null) return;
        DoEffect(context);
        context.Opponent.fieldOfPlay[context.PlayIndex].ChangeSuit(null);
    }
}
