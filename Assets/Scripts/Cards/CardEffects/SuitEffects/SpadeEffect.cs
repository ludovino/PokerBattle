using System.Collections;
using UnityEngine;

public class SpadeEffect : SuitEffect, IOnPlay, IOnHoverEnter
{
    public int damage;
    public static string indicatorName = "spade";
    public GameObject indicatorPrefab;

    public void OnPlay(CardEffectContext context)
    {
        if (context.OpposingCard is null) return;
        var changeCR = context.OpposingCard.ChangeValue(-damage);
        if (changeCR == null) return;
        CoroutineQueue.Defer(Effect_CR(changeCR, context));
    }

    public IEnumerator Effect_CR(IEnumerator changeCR, CardEffectContext context)
    {
        DoEffect(context);
        yield return changeCR;
    }

    public GameObject OnHoverEnter(CardEffectContext context) 
    {
        if(context.OpposingCard is null) return null;
        var indicator = Instantiate(indicatorPrefab);
        return indicator;
    }

    public void OnHoverExit(CardEffectContext context){}

    public override void Trigger(CardEffectContext context) {}
}

 