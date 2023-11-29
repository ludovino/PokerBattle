
public class FishEffect : SuitEffect, IOnPlay
{
    public void OnPlay(CardEffectContext context)
    {
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        context.Owner.Draw(1);
    }
}