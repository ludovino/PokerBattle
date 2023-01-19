using TMPro;
using UnityEngine;

public class ScoreLine : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _score;

    public void SetScore(int score, string name, int? quantity = null)
    {
        _score.text = score.ToString();
        var countPrefix = quantity != null ? $"[{quantity}] " : string.Empty;
        _title.text = countPrefix + name + ":";
    }
}