public class Evaluation
{
    public RankedHand playerHand;
    public RankedHand enemyHand;
    public Result result;
    private int _kickerCount;
    public int KickerCount => _kickerCount;
    public Evaluation(RankedHand playerHand, RankedHand enemyHand)
    {
        this.playerHand = playerHand;
        this.enemyHand = enemyHand;
        result = (Result)playerHand.CompareTo(enemyHand);
        _kickerCount = GetEvaluatedKickers();
    }

    private int GetEvaluatedKickers()
    {
        if (result == Result.Draw) return 0;
        if (playerHand.rank != enemyHand.rank) return 0;
        if (playerHand.hand.rankingCardsCount == 5) return 0;
        for (int i = 0; i < 5; i++)
        {
            if (playerHand.cards[i].highCardRank > enemyHand.cards[i].highCardRank) return i + 1 - playerHand.hand.rankingCardsCount;
            if (playerHand.cards[i].highCardRank < enemyHand.cards[i].highCardRank) return i + 1 - playerHand.hand.rankingCardsCount;
        }
        return 0;
    }

    public RankedHand winningHand => result == Result.PlayerLose ? enemyHand : result == Result.PlayerWin ? playerHand : null;
    public RankedHand losingHand => result == Result.PlayerWin ? enemyHand : result == Result.PlayerLose ? playerHand : null;

    public override string ToString()
    {
        if (result == Result.Draw) return "Drawn";
        else return $"{winningHand} beats {losingHand}";
    }
}

public enum Result
{
    PlayerWin = 1,
    PlayerLose = -1,
    Draw = 0
}