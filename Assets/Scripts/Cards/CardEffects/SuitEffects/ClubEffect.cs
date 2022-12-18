public class ClubEffect : CardEffect, IOnWinHand
{
    public override void Execute(PlayContext context)
    {
        if (context.Opponent.chips <= 0) return;
        DoEffect(context);
        context.Battle.AddToPot(context.Opponent, context.Card.blackjackValue);
    }
}
