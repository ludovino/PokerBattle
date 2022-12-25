
public class FishEffect : CardEffect, IOnPlay
{
    public override void Execute(PlayContext context)
    {
        DoEffect(context);
        context.Owner.Draw(1);
    }
}