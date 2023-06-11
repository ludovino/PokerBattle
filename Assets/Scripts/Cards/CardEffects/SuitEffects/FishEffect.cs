
public class FishEffect : SuitEffect, IOnPlay
{
    public override void Trigger(CardEffectContext context)
    {
        DoEffect(context);
        context.Owner.Draw(1);
    }
}