using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BattleController : MonoBehaviour
{
    private StateMachine _sm;
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
    public UnityEvent boss;
    [SerializeField]
    private OnChangeChips onChangePot;
    [SerializeField]
    private CardFactory _cardFactory;

    private int cardsPlayed;
    
    private void Awake()
    {
        onEvaluate = onEvaluate ?? new OnEvaluate();
        playerWin = playerWin ?? new UnityEvent();
        playerLose = playerLose ?? new UnityEvent();
        onChangePot = onChangePot ?? new OnChangeChips();
    }
    void ChangePot(int change)
    {
        var startingAmount = pot;
        pot += change;
        var currentAmount = pot;
        onChangePot.Invoke(startingAmount, currentAmount, change);

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

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        _sm = new StateMachine(new StartBattle());
        _sm.RegisterTransition<StartBattle, PlayerTurn>();
        _sm.RegisterTransition<PlayerTurn, PlayerTurn>();
        
        Init(GameController.Instance?.PlayerData ?? player.entityData, GameController.Instance?.NextBattle ?? (EnemyData)enemy.entityData);
    }
    public void Init(EntityData playerData, EnemyData enemyData)
    {
        var blind = GameController.GetBlind();
        player.Init(playerData, blind);
        enemy.Init(enemyData, blind);
        onChangePot.Invoke(0, 0, 0);
        _houseCutText.text = $"{houseCut * 100}% house cut";
        _blindText.text = $"{blind} blind";
        StartRound();
        if(GameController.Instance.currentAct.IsBossLevel)
        {
            boss.Invoke();
        }
    }

    private void MoveToState(IState state)
    {
        StartCoroutine(CR_MoveToState(state));
    }

    private IEnumerator CR_MoveToState(IState state)
    {
        yield return new WaitForEndOfFrame();
        _sm.MoveToState(state);
    }

    public void StartRound()
    {
        var result = Random.Range(0, 2) == 1;
        active = result ? player : enemy;
        idle = result ? enemy : player;
        MoveToState(new PlayerTurn(this));
    }

    private void Evaluate()
    {
        var playerHand = player.Evaluate();
        var enemyHand = enemy.Evaluate();
        var wld = playerHand.CompareTo(enemyHand);
        

        var eval = new Evaluation(playerHand, enemyHand);
        var winner = eval.result == Result.PlayerWin ? player : eval.result == Result.PlayerLose ? enemy : null;
        var loser = eval.result == Result.PlayerLose ? player : eval.result == Result.PlayerWin ? enemy : null;

        Debug.Log(eval.ToString());
        var loss = Mathf.Min(loser.chips, eval.winningHand.chipCost);
        onEvaluate.Invoke(eval);
        loser.ChangeChips(-loss);
        ChangePot(loss);
        TakeHouseCut();
        TakePot(winner);
        player.ClearField();
        enemy.ClearField();
        if (player.chips <= 0)
        {
            playerLose.Invoke();
            GameController.Instance.GameOver();
            return;
        }
        if (enemy.chips <= 0)
        {
            playerWin.Invoke();
            player.controller.ChooseCards(
                _cardFactory.GetRandomCards(5), 
                1, 
                cards => {
                    cards.ForEach(card => player.entityData.AddCard(card));
                    if (GameController.Instance.currentAct.IsBossLevel)
                    {
                        GameController.Instance.PlayerWins();
                        return;
                    }
                    GameController.Instance.GoToNextLevel();
                });
            
            return;
        }
        StartRound();
    }

    private void TakeHouseCut()
    {
        var change = pot * houseCut;
        ChangePot((int)-change);
    }

    public bool Play(int slotNumber, CardScript card)
    {
        bool played = active.Play(slotNumber, card);
        if(played) cardsPlayed++;
        return played;
    }
    public bool CanEndTurn()
    {
        if(player != active) return false;
        var opponentFull = idle.slotsRemaining == 0;
        if (opponentFull && active.slotsRemaining == 0) return true;
        if (cardsPlayed == 0 || opponentFull || active.chips == 0 && active.slotsRemaining > 0)
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
        if(cardsPlayed == 0 || opponentFull || active.chips == 0 && active.slotsRemaining > 0)
        {
            return false;
        }
        var swap = active;
        active = idle;
        idle = swap;
        MoveToState(new PlayerTurn(this));
        return true;
    }

    private class StartBattle : IState
    {
        public void OnEnter(){}
        public void OnExit(){}
    }

    private class PlayerTurn : IState
    {
        private BattleController _bc;
        private Entity _player;
        public PlayerTurn(BattleController battleController)
        {
            _bc = battleController;
        }
        public void OnEnter()
        {
            _bc.cardsPlayed = 0;
            _player = _bc.active;
            _player.Draw();
            var pay = Mathf.Min(_player.blind, _player.chips);
            _player.ChangeChips(-pay);
            if (_player.chips <= 0) _player.AllIn();
            _bc.ChangePot(pay);
            _player.controller.StartTurn();
        }
        public void OnExit()
        {
            _player.DiscardHand();
        }
    }
}

[System.Serializable]
public class OnEvaluate : UnityEvent<Evaluation>{}