
public class DiamondEffect : SuitEffect, IOnOpponentTurn
{
    public void OnOpponentTurn(CardEffectContext context)
    {
        if (context.OpposingCard != null) return;
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        if (context.Opponent.chips <= 0) return;
        context.Battle.AddToPot(context.Opponent, context.Opponent.blind);
    }
}

