
public class VoidEffect : SuitEffect, IOnOpponentPlay
{
    public void OnOpponentPlay(CardEffectContext context)
    {
        Trigger(context);
    }

    public override void Trigger(CardEffectContext context)
    {
        if (context.OpposingCard is null) return;
        if (context.OpposingCard.suit is null) return;
        DoEffect(context);
        context.Opponent.fieldOfPlay[context.PlayIndex].ChangeSuit(null);
    }
}
