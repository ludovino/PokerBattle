using UnityEngine;

public class ClubEffect : SuitEffect, IOnPlay
{
    public void OnPlay(CardEffectContext context)
    {
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        if (context.Opponent.chips <= 0) return;
        context.Battle.AddToPot(context.Opponent, Mathf.Clamp(context.Card.blackjackValue, 1, 21));
    }
}
