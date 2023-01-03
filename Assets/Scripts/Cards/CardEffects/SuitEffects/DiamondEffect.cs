
public class DiamondEffect : SuitEffect, IOnOpponentTurn
{
    public override void Execute(CardEffectContext context)
    {
        if (context.Opponent.chips <= 0) return;
        DoEffect(context);
        context.Battle.AddToPot(context.Opponent, context.Opponent.blind);
    }
}

