using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BattleController : MonoBehaviour
{
    [SerializeField]
    private Entity player;
    [SerializeField]
    private Entity enemy;
    private Entity active;
    private Entity idle;
    private int pot;
    [SerializeField]
    private float houseCut;
    [SerializeField]
    private TextMeshPro _houseCutText;
    [SerializeField]
    private TextMeshPro _blindText;

    public OnEvaluate onEvaluate;
    public UnityEvent playerWin;
    public UnityEvent playerLose;
    public OnCoinToss onCoinToss;
    public UnityEvent boss;
    [SerializeField]
    private OnChangeChips onChangePot;
    [SerializeField]
    private CardPool _cardFactory;
    
    [SerializeField]
    private RewardScreen _rewardScreen;
    [SerializeField]
    private RewardGenerator _rewardGenerator;
    [SerializeField]
    private ScoreKeeper _scoreKeeper;

    private int cardsPlayed;
    private int startingBlind;
    public int StartingBlind => startingBlind;
    private int blind;

    private void Awake()
    {
        onEvaluate ??= new OnEvaluate();
        playerWin ??= new UnityEvent();
        playerLose ??= new UnityEvent();
        onChangePot ??= new OnChangeChips();
        onCoinToss ??= new OnCoinToss();
    }
    void ChangePot(int change)
    {
        var startingAmount = pot;
        pot += change;
        var currentAmount = pot;
        onChangePot.Invoke(startingAmount, currentAmount, change);

    }

    public void AddToPot(Entity entity, int amount)
    {
        var change = Mathf.Min(amount, entity.chips);
        entity.ChangeChips(-change);
        ChangePot(change);
    }

    private void TakePot(Entity taker)
    {
        var startingAmount = pot;
        var currentAmount = 0;
        var change = -pot;
        pot = 0;
        onChangePot.Invoke(startingAmount, currentAmount, change);
        taker.ChangeChips(startingAmount);
    }

    public void TakeFromPot(Entity taker, int amount)
    {
        var change = Mathf.Min(amount, pot);
        var startingAmount = pot;
        var currentAmount = pot - amount;
        pot = currentAmount;
        onChangePot.Invoke(startingAmount, currentAmount, -change);
        taker.ChangeChips(change);
    }

    private void SplitPot()
    {
        var startingAmount = pot;
        var currentAmount = 0;
        var change = -pot;
        var half = Mathf.FloorToInt(startingAmount * 0.5f);
        pot = 0;
        onChangePot.Invoke(startingAmount, currentAmount, change);
        player.ChangeChips(half);
        enemy.ChangeChips(half);
    }

    public void Init()
    {
        Init(player.entityData, (EnemyData)enemy.entityData, new List<RewardGenerator>() { _rewardGenerator });
    }

    public void Init(EntityData playerData, EnemyData enemyData, List<RewardGenerator> rewards)
    {
        _rewardScreen.Init(rewards);

        startingBlind = GameController.GetBlind();

        blind = startingBlind;
        player.Init(playerData, blind);
        enemy.Init(enemyData, blind);
        onChangePot.Invoke(0, 0, 0);
        if(_houseCutText != null) _houseCutText.text = $"{houseCut * 100}% house cut";
        if (_blindText != null) _blindText.text = $"{blind} blind";
        StartRound();
        if(GameController.Instance?.currentAct?.IsBossLevel ?? false)
        {
            boss.Invoke();
        }
    }

    public void StartRound()
    {
        var result = Random.Range(0, 2) == 1;
        active = result ? player : enemy;
        idle = result ? enemy : player;
        StartTurn();
    }

    public void StartTurn()
    {
        cardsPlayed = 0;
        active.Draw();
        var pay = Mathf.Min(active.blind, active.chips);
        idle.OnOpponentTurnStart();
        active.OnTurnStart();
        AddToPot(active, pay);
        if (active.chips <= 0) active.AllIn();
        active.controller.StartTurn();
    }

    private void Evaluate()
    {
        IncreaseBlinds();
        
        player.DiscardHand();
        enemy.DiscardHand();
        var playerHand = player.Evaluate();
        var enemyHand = enemy.Evaluate();
        var wld = playerHand.CompareTo(enemyHand);

        var eval = new Evaluation(playerHand, enemyHand);
        var winner = eval.result == Result.PlayerWin ? player : eval.result == Result.PlayerLose ? enemy : null;
        var loser = eval.result == Result.PlayerLose ? player : eval.result == Result.PlayerWin ? enemy : null;
        
        if(eval.result == Result.PlayerWin)
        {
            _scoreKeeper.WinHand(playerHand);
        }

        Debug.Log(eval.ToString());
        onEvaluate.Invoke(eval);
        
        if(eval.result == Result.Draw)
        {
            TakeHouseCut();
            SplitPot();
        }
        else
        {
            eval.winningHand.rankingCards.ForEach(c =>  c.DoWinEffects());
            TakeHouseCut();
            TakePot(winner);
        }
        player.ClearField();
        enemy.ClearField();
        if (player.chips <= 0)
        {
            Lose();
            return;
        }
        if (enemy.chips <= 0)
        {
            _scoreKeeper.WinTable();
            ShowRewards();
            return;
        }
        StartRound();
    }
    public void ShowRewards()
    {
        _rewardScreen.Open();
    }

    public void Win()
    {
        playerWin.Invoke();
    }

    public void Lose()
    {
        playerLose.Invoke();
    }

    private void IncreaseBlinds()
    {
        blind += startingBlind;
        player.blind = blind;
        enemy.blind = blind;
        if (_blindText != null) _blindText.text = $"{blind} blind";
    }

    private void TakeHouseCut()
    {
        var change = pot * houseCut;
        ChangePot((int)-change);
    }

    public bool Play(int slotNumber, CardScript card)
    {
        bool played = active.Play(slotNumber, card);
        idle.OpponentPlayed(slotNumber);
        if (!played) return played;
        cardsPlayed++;
        card.Play(new CardEffectContext(this, active, idle, card, slotNumber));
        return played;
    }
    public bool CanEndTurn()
    {
        if(player != active) return false;
        if (cardsPlayed == 0 || active.chips == 0 && active.slotsRemaining > 0)
        {
            return false;
        }
        return true;
    }

    public bool EndTurn()
    {
        var opponentFull = idle.slotsRemaining == 0;
        if(opponentFull && active.slotsRemaining == 0)
        {
            Evaluate();
            return true;
        }
        if(cardsPlayed == 0 || active.chips == 0 && active.slotsRemaining > 0)
        {
            return false;
        }

        active.DiscardHand();

        if (!opponentFull)
        {
            var swap = active;
            active = idle;
            idle = swap;
        }

        StartTurn();
        return true;
    }
}

[System.Serializable]
public class OnEvaluate : UnityEvent<Evaluation>{}
public class OnCoinToss : UnityEvent<int>{}

public class CardEffectContext
{
    private BattleController _battle;
    private Entity _owner;
    private Entity _opponent;
    private CardScript _card;
    private int _playIndex;

    public CardEffectContext(BattleController battle, Entity owner, Entity opponent, CardScript card, int playIndex = -1)
    {
        _battle = battle;
        _owner = owner;
        _opponent = opponent;
        _card = card;
        _playIndex = playIndex;
    }

    public Entity Owner => _owner;
    public Entity Opponent => _opponent;
    public CardScript OpposingCard => Opponent.fieldOfPlay[PlayIndex];
    public CardScript Card => _card;
    public BattleController Battle => _battle;
    public int PlayIndex  => _playIndex;
}