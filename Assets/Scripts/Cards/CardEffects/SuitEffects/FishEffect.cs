
public class FishEffect : SuitEffect, IOnPlay
{
    public override void Execute(CardEffectContext context)
    {
        DoEffect(context);
        context.Owner.Draw(1);
    }
}