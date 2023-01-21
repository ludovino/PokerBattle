using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using static MetaProgress;

[CreateAssetMenu(menuName = "Metaprogression")]
public partial class MetaProgress : ScriptableObject, ISaveable<MetaprogressData>
{
    private static MetaProgress _instance;
    public static MetaProgress Instance => _instance != null ? _instance : SetInstance();

    [SerializeField] 
    public SuitList _defaultSuits;
    [SerializeField]
    private SuitList _AllDrawableSuits;
    void OnEnable()
    {
        SetInstance();
        UpdateUnlockedRelics();
        SaveManager.Instance.LoadItem(this);
    }

    private void OnValidate()
    {
        if (_suitUnlocks == null) _suitUnlocks = new List<SuitUnlocks>();
        var missingSuits = _AllDrawableSuits.Except(_suitUnlocks.Select(su => su.Suit));
        _suitUnlocks.AddRange(missingSuits.Select(s => new SuitUnlocks(s)));
    }

    [SerializeField]
    private int _totalScore;
    public int TotalScore => _totalScore;

    [SerializeField]
    private List<Unlockable> _totalUnlocks;

    [SerializeField]
    private List<Relic> _defaultRelics;

    private List<Relic> _unlockedRelics;
    public IReadOnlyList<Relic> UnlockedRelics => _unlockedRelics;

    public string UniqueName => "METAPROGRESS";

    [SerializeField]
    [FormerlySerializedAs("_suitScores")]
    private List<SuitUnlocks> _suitUnlocks;
    private static MetaProgress SetInstance()
    {
        return _instance = Resources.Load<MetaProgress>("Unlocks/Metaprogression");
    }

    public IReadOnlyList<Suit> UnlockedSuits => _defaultSuits.Concat(_totalUnlocks
        .OfType<UnlockableSuit>()
        .Where(us => us.Score <= _totalScore)
        .Select(us => us.Suit)).ToList();

    private void UpdateUnlockedRelics()
    {
        _unlockedRelics.Clear();
        _unlockedRelics.AddRange(_defaultRelics);
        _unlockedRelics.AddRange(_totalUnlocks.OfType<UnlockableRelic>()
        .Where(ur => ur.Score <= _totalScore)
        .Select(ur => ur.Relic));
        var unlockedSuitRelics = _suitUnlocks.SelectMany(su => su.Unlocked.OfType<UnlockableRelic>().Select(ur => ur.Relic));
        _unlockedRelics.AddRange(unlockedSuitRelics);
    }
    internal void AddToScores(int score, List<SuitScore> suitScores)
    {
        _totalScore += score;
        foreach(var suitScore in suitScores)
        {
            var suitUnlocks = _suitUnlocks.Single(s => s.Suit == suitScore.Suit);
            suitUnlocks.AddPoints(suitScore.Score);
        }
        UpdateUnlockedRelics();
        SaveManager.Instance.SaveItem(this);
    }

    public MetaprogressData Save()
    {
        return new MetaprogressData { 
            totalScore = _totalScore, 
            suitScores = _suitUnlocks
                .Select(s => s.ToString())
                .ToList() 
        };
    }
    public void SetSuitUnlockFromString(string serializedString)
    {
        var suitLetter = serializedString.First().ToString();
        var countString = string.Join("",serializedString.Skip(1));
        var suitScore = _suitUnlocks.Single(s => s.Suit.shortName == suitLetter);
        suitScore.SetScore(int.Parse(countString));
    }

    public void Load(MetaprogressData data)
    {
        if (data is null) return;
        _totalScore = data.totalScore;
        foreach(var suitScore in data.suitScores)
        {
            SetSuitUnlockFromString(suitScore);
        }
    }

    [Serializable]
    private class SuitUnlocks
    {
        public SuitUnlocks(Suit suit) 
        { 
            _suit = suit; 
            _score = 0;
        }

        public SuitUnlocks(Suit suit, int score)
        {
            _suit = suit;
            _score = score;
        }

        [SerializeField]
        private Suit _suit;
        public Suit Suit => _suit;
        [SerializeField]
        private int _score;
        public int Score => _score;
        public void AddPoints(int score) => _score += score;
        public void SetScore(int score) => _score = score;
        [SerializeField]
        private List<Unlockable> _unlocks;
        public List<Unlockable> Unlockables => _unlocks;
        public List<Unlockable> Unlocked => _unlocks.Where(u => u.Score <= _score).ToList();
        public override string ToString()
        {
            return $"{Suit.shortName}{Score}";
        }
    }

    public class MetaprogressData
    {
        public int totalScore;
        public List<string> suitScores;
    }
}