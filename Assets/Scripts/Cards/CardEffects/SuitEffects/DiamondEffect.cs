﻿
public class DiamondEffect : SuitEffect, IOnOpponentTurn
{
    public override void Trigger(CardEffectContext context)
    {
        if (context.Opponent.chips <= 0) return;
        if (context.OpposingCard != null) return;
        DoEffect(context);
        context.Battle.AddToPot(context.Opponent, context.Opponent.blind);
    }
}

