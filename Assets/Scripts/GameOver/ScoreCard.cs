using Animancer;
using System.Linq;
using TMPro;
using UnityEngine;
using static Animancer.Validate;

public class ScoreCard : MonoBehaviour
{
    [SerializeField]
    private ScoreLine _scorelinePrefab;
    [SerializeField]
    private Transform _scoreList;
    [SerializeField]
    private TextMeshProUGUI _runTotalText;
    [SerializeField]
    private TextMeshProUGUI _overallTotalText;
    [SerializeField]
    private ScoreKeeper _scoreKeeper;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        int totalScore = 0;
        var scoreline = Instantiate(_scorelinePrefab, _scoreList);
        var tablesScore = _scoreKeeper.TablePlayedScore * _scoreKeeper.TablesBeaten;
        totalScore += tablesScore;
        scoreline.SetScore(tablesScore, "Tables won", _scoreKeeper.TablesBeaten);

        foreach (var act in _scoreKeeper.CompletedActs)
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            totalScore += act.Score;
            scoreline.SetScore(act.Score, act.DisplayName);
        }

        foreach (var suitScore in _scoreKeeper.SuitScores.Where(kvp => kvp.Value > 0))
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            totalScore += suitScore.Value;
            scoreline.SetScore(suitScore.Value, suitScore.Key.longName);
        }
        scoreline = Instantiate(_scorelinePrefab, _scoreList);
        scoreline.SetScore(_scoreKeeper.BlankScore, "Blank");

        foreach (var handScore in _scoreKeeper.HandCount.Where(kvp => kvp.Value > 0))
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            var totalHandScore = handScore.Value * handScore.Key.score;
            totalScore += totalHandScore;
            scoreline.SetScore(totalHandScore, handScore.Key.DisplayName, handScore.Value);
        }
        _overallTotalText.text = $"Total: {MetaProgress.Instance.TotalScore}";
        _runTotalText.text = $"This run: {totalScore}";
    }
}