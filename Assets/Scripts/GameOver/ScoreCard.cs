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

    public void Init()
    {
        SetRunText(0);
        SetTotalText(MetaProgress.Instance.TotalScore - _scoreKeeper.GetScore());
        CoroutineQueue.Defer(CR_ShowScore());
    }

    public IEnumerator CR_ShowScore()
    {
        var animatedMetaScore = MetaProgress.Instance.TotalScore - _scoreKeeper.GetScore();
        var animatedScore = 0;
        var totalScore = _scoreKeeper.GetScore();
        yield return new WaitForSeconds(0.7f);

        ScoreLine scoreline;
        foreach (var lineItem in _scoreKeeper.LineItems)
        {
            scoreline = Instantiate(_scorelinePrefab, _scoreList);
            StartCoroutine(scoreline.CR_SetScore(lineItem.score, lineItem.itemName, lineItem.count));
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