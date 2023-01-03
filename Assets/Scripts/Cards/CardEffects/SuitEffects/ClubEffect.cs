public class ClubEffect : SuitEffect, IOnWinHand
{
    public override void Execute(CardEffectContext context)
    {
        if (context.Opponent.chips <= 0) return;
        DoEffect(context);
        context.Battle.AddToPot(context.Opponent, context.Card.blackjackValue);
    }
}
