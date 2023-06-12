using UnityEngine;

public class SpadeEffect : SuitEffect, IOnPlay
{
    public int damage;

    public void OnPlay(CardEffectContext context)
    {
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        if (context.OpposingCard is null) return;
        DoEffect(context);
        context.OpposingCard.ChangeValue(-damage);
    }
}

