using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private StateMachine _sm;
    public static GameController Instance { get; private set; }
    [SerializeField]
    private MusicManager music;
    public ActConfiguration currentAct;
    [SerializeField]
    private EntityData _playerData;
    public EntityData PlayerData => _playerData;
    public Scenario _startingScenario;
    public MetaProgress metaProgress;
    public ScoreKeeper scoreKeeper;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _sm = new StateMachine(new MainMenu());
        _sm.RegisterTransition<MainMenu, DrawDeck>();
        _sm.RegisterTransition<DrawDeck, MapEvent>();
        _sm.RegisterTransition<Map, MapEvent>();
        _sm.RegisterTransition<MapEvent, Map>();
        _sm.RegisterTransition<Map, Battle>();
        _sm.RegisterTransition<Map, Map>();
        _sm.RegisterTransition<Map, Shop>();
        _sm.RegisterTransition<Shop, Map>();
        _sm.RegisterTransition<Battle, Lose>();
        _sm.RegisterTransition<Battle, Win>();
        _sm.RegisterTransition<Lose, MainMenu>();
        _sm.RegisterTransition<Win, MainMenu>();
        _sm.RegisterTransition<Battle, Map>();
    }
    public void BeginGame()
    {
        InitObjects();
        currentAct.level = -1;
        _sm.MoveToState(new DrawDeck());
    }

    private void InitObjects()
    {
       var assets = Resources.LoadAll<ScriptableObject>("").OfType<IOnInit>().ToArray();
        foreach ( var asset in assets)
        {
            asset.Init();
        }
    }

    public void DeckChosen()
    {
        music.Game();
        _sm.MoveToState(new MapEvent(_startingScenario));
    }
    public void BeginBattle(EnemyData enemy, List<RewardGenerator> rewardGenerators)
    {
        if (currentAct.IsBossLevel) music.Boss();
        _sm.MoveToState(new Battle(_playerData, enemy, rewardGenerators));
    }
    public void GoToNextLevel()
    {
        currentAct.level++;
        _sm.MoveToState(new Map());
    }

    public static int GetBlind()
    {
        return Instance?.currentAct.GetLevel().blind ?? 5;
    }

    public void ReturnToMenu()
    {
        music.Menu();
        _sm.MoveToState(new MainMenu());
    }

    public void GameOver()
    {
        scoreKeeper.EndGame();
        CoroutineQueue.Defer(CR_GameOver());
    }

    public IEnumerator CR_GameOver()
    {
        music.StopMusic();
        _sm.MoveToState(new Lose());
        yield return null;
    }

    internal void PlayerWins()
    {
        scoreKeeper.EndGame();
        _sm.MoveToState(new Win());
    }

    internal void OpenShop()
    {
        _sm.MoveToState(new Shop());
    }

    internal void BeginScenario(Scenario scenario)
    {
        _sm.MoveToState(new MapEvent(scenario));
    }

    // main menu
    public class MainMenu : IState
    {
        public void OnEnter()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name != "MainMenu")
            {
                SceneChanger.Instance.ChangeScene("MainMenu");
            }
        }

        public void OnExit(){}
    }

    // draw deck
    public class DrawDeck : IState
    {
        public void OnEnter()
        {
            SceneChanger.Instance.ChangeScene("DrawDeck");
        }

        public void OnExit()
        {
        }
    }

    // overworld map
    public class Map : IState
    {
        public void OnEnter()
        {
            SceneChanger.Instance.ChangeScene("Map");
        }

        public void OnExit()
        {
            // Save State
        }
    }
    public class Shop : IState
    {
        public void OnEnter()
        {
            SceneChanger.Instance.ChangeScene("Shop");
        }

        public void OnExit()
        {
            // Save State
        }
    }
    // event

    // battle
    public class Battle : IState
    {
        private EnemyData _enemy;
        private EntityData _player;
        private List<RewardGenerator> _rewardGenerator;
        public Battle(EntityData playerData, EnemyData enemy, List<RewardGenerator> rewardGenerators)
        {
            _enemy = enemy;
            _player = playerData;
            _rewardGenerator = rewardGenerators;
        }

        public void OnEnter()
        {
            SceneChanger.Instance.ChangeScene("Battle", StartBattle);
        }

        void StartBattle()
        {
            FindObjectOfType<BattleController>().Init(_player, _enemy, _rewardGenerator);
        }
        public void OnExit()
        {}
    }

    // game over
    public class Lose : IState
    {
        public void OnEnter()
        {
            SceneChanger.Instance.ChangeScene("Loss", ShowScoreCard);
        }

        void ShowScoreCard()
        {
            FindObjectOfType<ScoreCard>().Init();
        }

        public void OnExit()
        {
        
        }
    }
    // game complete
    public class Win : IState
    {
        public void OnEnter()
        {
            SceneChanger.Instance.ChangeScene("Victory");
        }

        public void OnExit()
        {

        }
    }

    private class MapEvent : IState
    {
        private Scenario _scenario;

        public void OnEnter()
        {
            SceneChanger.Instance.ChangeScene("Event", StartScenario);
        }
        private void StartScenario()
        {
            FindObjectOfType<DecisionScenario>().Init(_scenario);
        }
        public MapEvent(Scenario scenario)
        {
            _scenario = scenario;
        }

        public void OnExit()
        {
            
        }
    }
}
