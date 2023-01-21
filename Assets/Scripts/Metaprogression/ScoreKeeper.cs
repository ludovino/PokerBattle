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

    public int HandScore => HandCount.Select(kvp => kvp.Value * kvp.Key.score).Sum();
    private Dictionary<Suit, int> _suitScores;
    public IReadOnlyDictionary<Suit, int> SuitScores => _suitScores;
    public int SuitScore => _suitScores.Select(kvp => kvp.Value).Sum() + _blankScore;
    
    private int _blankScore;
    public int BlankScore => _blankScore;

    private int tablesBeaten = 0;
    public int TablesBeaten => tablesBeaten;

    private List<ActConfiguration> completedActs;
    public IReadOnlyList<ActConfiguration> CompletedActs => completedActs;
    public void WinHand(RankedHand rankedHand)
    {
        handCount[rankedHand.hand] += 1;
        foreach(var card in rankedHand.rankingCards)
        {
            if (!card.suit)
            {
                _blankScore += card.blackjackValue;
                continue;
            }
            if(_playerSuitList.Contains(card.suit)) 
                _suitScores[card.suit] += card.blackjackValue;
            
        }
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
        completedActs = new List<ActConfiguration>();
    }

    public void EndGame()
    {
        var suitScores = _suitScores.Select(s => new SuitScore() { Suit = s.Key, Score = s.Value }).Where(s => s.Score > 0).ToList();
        var score = handCount.Select(kvp => kvp.Key.rank * kvp.Value).Sum();
        score += CompletedActs.Sum(ca => ca.Score);
        score += tablesBeaten * _tablePlayedScore;
        MetaProgress.Instance.AddToScores(score, suitScores);
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
