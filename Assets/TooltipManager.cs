using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _tooltip;
    [SerializeField]
    private TextMeshProUGUI _text;

    private void Start()
    {
        _tooltip.SetActive(false);
    }
    public void ShowTooltip(string message)
    {
        _text.text = message;
        _tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        _tooltip.SetActive(false);
    }
}
