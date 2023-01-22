using DG.Tweening;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

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
        CoroutineQueue.Defer(CR_ShowScore());
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

    public IEnumerator CR_ShowScore()
    {
        var animatedMetaScore = MetaProgress.Instance.TotalScore - _scoreKeeper.GetTotalScore();
        var animatedScore = 0;
        yield return new WaitForSeconds(0.7f);
        int totalScore = 0;
        var scoreline = Instantiate(_scorelinePrefab, _scoreList);
        var tablesScore = _scoreKeeper.TablePlayedScore * _scoreKeeper.TablesBeaten;
        totalScore += tablesScore;
        StartCoroutine(scoreline.CR_SetScore(tablesScore, "Tables won", _scoreKeeper.TablesBeaten));

        foreach (var act in _scoreKeeper.CompletedActs)
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            totalScore += act.Score;
            StartCoroutine(scoreline.CR_SetScore(act.Score, act.DisplayName));
        }

        foreach (var suitScore in _scoreKeeper.SuitScores.Where(kvp => kvp.Value > 0))
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            totalScore += suitScore.Value;
            StartCoroutine(scoreline.CR_SetScore(suitScore.Value, suitScore.Key.longName));
        }
        scoreline = Instantiate(_scorelinePrefab, _scoreList);
        StartCoroutine(scoreline.CR_SetScore(_scoreKeeper.BlankScore, "Blank"));

        foreach (var handScore in _scoreKeeper.HandCount.Where(kvp => kvp.Value > 0))
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            var totalHandScore = handScore.Value * handScore.Key.score;
            totalScore += totalHandScore;
            StartCoroutine(scoreline.CR_SetScore(totalHandScore, handScore.Key.DisplayName, handScore.Value));
        }
        yield return DOTween.To(() => animatedScore, x => animatedScore = x, totalScore, 1f).OnUpdate(() => SetRunText(animatedScore)).WaitForCompletion();

        yield return DOTween.To(() => animatedMetaScore, x => animatedMetaScore = x, MetaProgress.Instance.TotalScore, 1f).OnUpdate(() => SetTotalText(animatedMetaScore)).WaitForCompletion();
    }

    private void SetRunText(int score)
    {
        _runTotalText.text = $"This run: {score}";
    }
    private void SetTotalText(int score)
    {
        _overallTotalText.text = $"Total: {score}";
    }
}