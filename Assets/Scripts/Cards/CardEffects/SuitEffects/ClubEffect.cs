using UnityEngine;

public class ClubEffect : SuitEffect, IOnWinHand
{
    public void OnWinHand(CardEffectContext context)
    {
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        if (context.Opponent.chips <= 0) return;
        DoEffect(context);
        context.Battle.AddToPot(context.Opponent, Mathf.Clamp(context.Card.blackjackValue, 1, 21));
    }
}
