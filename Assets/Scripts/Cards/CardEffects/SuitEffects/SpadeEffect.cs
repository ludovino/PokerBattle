using System.Collections;
using UnityEngine;

public class SpadeEffect : SuitEffect, IOnPlay, IOnHoverEnter
{
    public int damage;
    public static string indicatorName = "spade";
    public GameObject indicatorPrefab;
    public CardChanger effectPrefab;
    public void OnPlay(CardEffectContext context)
    {
        Trigger(context);
    }

    public GameObject OnHoverEnter(CardEffectContext context) 
    {
        if(context.OpposingCard is null) return null;
        var indicator = Instantiate(indicatorPrefab);
        return indicator;
    }
   
    public void OnHoverExit(CardEffectContext context){}

    public override void Trigger(CardEffectContext context) {
        if (context.OpposingCard is null) return;
        var sprite = context.OpposingCard.ChangeValue(-damage);
        CoroutineQueue.Defer(Effect_CR(context.OpposingCard, sprite));
    }

    public IEnumerator Effect_CR(CardScript card, Sprite sprite)
    {
        var changed = false;
        var effect = Instantiate(effectPrefab);
        effect.transform.position = card.transform.position;
        effect.Target = card;
        effect.Sprite = sprite;
        effect.onChanged.AddListener(() => changed = true);
        yield return new WaitUntil(() => changed);
    }
}

 