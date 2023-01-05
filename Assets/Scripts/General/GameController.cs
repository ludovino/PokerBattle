using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System.Collections;
using System;

public class GameController : MonoBehaviour
{
    private StateMachine _sm;
    public static GameController Instance { get; private set; }
    [SerializeField]
    private MusicManager music;
    public ActConfiguration currentAct;
    public EnemyData NextBattle { get; private set; }
    [SerializeField]
    private EntityData _playerData;
    public EntityData PlayerData => _playerData;
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
        _sm.RegisterTransition<DrawDeck, Map>();
        _sm.RegisterTransition<Map, Battle>();
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
        currentAct.level = 0;
        _sm.MoveToState(new DrawDeck());
    }

    private void InitObjects()
    {
       var assets = Resources.LoadAll<ScriptableObject>("").Where(so => so is IOnInit).Cast<IOnInit>().ToArray();
        foreach ( var asset in assets)
        {
            asset.Init();
        }
    }

    public void DeckChosen()
    {
        music.Game();
        _sm.MoveToState(new Map());
    }
    public void BeginBattle(EnemyData enemy)
    {
        NextBattle = enemy;
        if (currentAct.IsBossLevel) music.Boss();
        _sm.MoveToState(new Battle(_playerData, enemy));
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
        _sm.MoveToState(new Win());
    }

    internal void OpenShop()
    {
        _sm.MoveToState(new Shop());
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
        EntityData _player;

        public Battle(EntityData playerData, EnemyData enemy)
        {
            _enemy = enemy;
            _player = playerData;
        }

        public void OnEnter()
        {
            SceneChanger.Instance.ChangeScene("Battle", StartBattle);
        }
        void StartBattle()
        {
            FindObjectOfType<BattleController>().Init(_player, _enemy);
        }
        public void OnExit()
        {}
    }

    // game over
    public class Lose : IState
    {
        public void OnEnter()
        {
            SceneChanger.Instance.ChangeScene("Loss");
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
}
