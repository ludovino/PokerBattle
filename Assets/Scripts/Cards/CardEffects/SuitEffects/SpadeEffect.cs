using UnityEngine;

public class SpadeEffect : SuitEffect, IOnPlay
{
    public int damage;
    public override void Trigger(CardEffectContext context)
    {
        if (context.OpposingCard is null) return;
        DoEffect(context);
        context.OpposingCard.ChangeValue(-damage);
    }
}

