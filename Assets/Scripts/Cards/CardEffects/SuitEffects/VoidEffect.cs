
public class VoidEffect : SuitEffect, IAfterOpponentPlay
{
    public override void Execute(CardEffectContext context)
    {
        var card = context.Opponent.fieldOfPlay[context.PlayIndex];
        if (card.suit is null) return;
        DoEffect(context);
        context.Opponent.fieldOfPlay[context.PlayIndex].ChangeSuit(null);
    }
}
