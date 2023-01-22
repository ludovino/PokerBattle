using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreLine : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _score;
    private int _scoreNumber;
    public void SetScore(int score, string name, int? quantity = null)
    {
        _scoreNumber = score;
        SetScore(_scoreNumber);
        var countPrefix = quantity != null ? $"[{quantity}] " : string.Empty;
        _title.text = countPrefix + name + ":";
    }

    public void SetScore(int score) => _score.text = score.ToString();
    

    public IEnumerator CR_SetScore(int score, string name, int? quantity = null)
    {
        var countPrefix = quantity != null ? $"[{quantity}] " : string.Empty;
        _title.text = countPrefix + name + ":";
        var tween = DOTween.To(() => _scoreNumber, x => _scoreNumber = x, score, 0.5f);
        while (tween.IsPlaying())
        {
            SetScore(_scoreNumber);
            yield return null;
        }
        SetScore(_scoreNumber);
    }
}