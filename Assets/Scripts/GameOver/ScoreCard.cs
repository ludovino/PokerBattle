using TMPro;
using UnityEngine;

public class ScoreCard : MonoBehaviour
{
    [SerializeField]
    private ScoreLine _scorelinePrefab;
    [SerializeField]
    private Transform _scoreList;
    [SerializeField]
    private TextMeshProUGUI _totalText;
    [SerializeField]
    private ScoreKeeper _scoreKeeper;

    public void Init()
    {
        var scoreline = Instantiate(_scorelinePrefab, _scoreList);
        scoreline.SetScore(_scoreKeeper.TablePlayedScore * _scoreKeeper.TablesBeaten, "Tables won", _scoreKeeper.TablesBeaten);

        foreach (var act in _scoreKeeper.CompletedActs)
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            scoreline.SetScore(act.Score, act.DisplayName);
        }

        foreach(var suitScore in _scoreKeeper.SuitScores)
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            scoreline.SetScore(suitScore.Value, suitScore.Key.longName);
        }
        scoreline = Instantiate(_scorelinePrefab, _scoreList);
        scoreline.SetScore(_scoreKeeper.BlankScore, "Blank");

        foreach (var handScore in _scoreKeeper.HandCount)
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            scoreline.SetScore(handScore.Value * handScore.Key.score, handScore.Key.DisplayName, handScore.Value);
        }
    }
}