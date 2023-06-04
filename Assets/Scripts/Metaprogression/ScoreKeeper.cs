using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scorekeeper")]
public class ScoreKeeper : ScriptableObject, IOnInit
{
    [SerializeField]
    private SuitList _playerSuitList;

    [SerializeField]
    private int _tablePlayedScore;
    public int TablePlayedScore => _tablePlayedScore;
    private Dictionary<PokerHand, int> handCount;
    public IReadOnlyDictionary<PokerHand, int> HandCount => handCount;
    private Dictionary<Suit, int> _suitScores;
    public IReadOnlyDictionary<Suit, int> SuitScores => _suitScores;

    private int _blankScore;
    public int BlankScore => _blankScore;

    private int tablesBeaten = 0;
    public int TablesBeaten => tablesBeaten;

    private List<ActConfiguration> completedActs;
    public IReadOnlyList<ActConfiguration> CompletedActs => completedActs;
    public void WinHand(RankedHand rankedHand)
    {
        if (!handCount.ContainsKey(rankedHand.hand)) handCount.Add(rankedHand.hand, 0);
        handCount[rankedHand.hand] += 1;
        foreach (var card in rankedHand.rankingCards)
        {
            if (!card.suit)
            {
                _blankScore += Mathf.Clamp(card.blackjackValue, 1, 21);
                continue;
            }
            if (_playerSuitList.Contains(card.suit))
                _suitScores[card.suit] += Mathf.Clamp(card.blackjackValue, 1, 21);
        }
    }
    
    private List<ScoreLineItem> _lineItems;
    public IEnumerable<ScoreLineItem> LineItems => (_lineItems?.Count ?? 0) > 0 ? _lineItems : SetLineItems();
    private IEnumerable<ScoreLineItem> SetLineItems()
    {
        _lineItems ??= new List<ScoreLineItem>();
        _lineItems.Clear();
        
        // tables
        _lineItems.Add(new ScoreLineItem() 
        { 
            count = tablesBeaten, 
            itemName = "Tables Beaten", 
            score = tablesBeaten * TablePlayedScore 
        });

        // blanks
        _lineItems.Add(new ScoreLineItem() 
        { 
            itemName = "Blank", 
            score = _blankScore 
        });
     
        // suits
        _lineItems.AddRange(_suitScores.Select(kvp => new ScoreLineItem() 
        { 
            itemName = kvp.Key.longName, 
            score = kvp.Value 
        }));

        // hands
        _lineItems.AddRange(handCount.Select(kvp => new ScoreLineItem()
        {
            count = kvp.Value,
            itemName = kvp.Key.DisplayName,
            score = kvp.Value * kvp.Key.score
        }));
        _lineItems.RemoveAll(li => li.score == 0);
        return _lineItems;
    }

    public void WinTable()
    {
        tablesBeaten++;
    }
    public void DefeatAct(ActConfiguration act)
    {
        completedActs.Add(act);
    }

    public void Init()
    {
        var hands = Resources.LoadAll<PokerHand>("");
        handCount = hands.ToDictionary(h => h, h => 0);

        var suits = Resources.LoadAll<Suit>("");
        _suitScores = suits.ToDictionary(s => s, s => 0);

        completedActs ??= new List<ActConfiguration>();
        completedActs.Clear();

        _lineItems ??= new List<ScoreLineItem>();
        _lineItems.Clear();
    }

    public void EndGame()
    {
        List<SuitScore> suitScores = GetSuitScores();
        int score = GetScore();
        MetaProgress.Instance.AddToScores(score, suitScores);
    }

    private List<SuitScore> GetSuitScores()
    {
        return _suitScores.Select(s => new SuitScore() { Suit = s.Key, Score = s.Value }).Where(s => s.Score > 0).ToList();
    }

    public int GetScore()
    {
        return LineItems.Sum(li => li.score);
    }

    public class RunStats
    {
        public Dictionary<PokerHand, int> handCount;
        public Dictionary<Suit, int> suitScore;
        public List<ActConfiguration> completedActs;
        public int tablesBeaten;
    }

    int totalScore => TablePlayedScore * TablesBeaten + _blankScore;
}

public class ScoreLineItem
{
    public int? count;
    public string itemName;
    public int score;
}