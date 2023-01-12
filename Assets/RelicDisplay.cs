using UnityEngine;
using UnityEngine.UI;

public class RelicDisplay : MonoBehaviour
{
    private Image _image;
    [SerializeField]
    private Relic _relic;
    private SimpleTooltip _tooltip;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _tooltip = GetComponent<SimpleTooltip>();
    }

    private void Start()
    {
        Debug.Log($"IS EDITOR?? {Application.isEditor}");
        if(_relic && Application.isEditor)
        {
            Init(_relic);
        }
    }

    public void Init(Relic relic)
    {
        _relic = relic;
        _image.sprite = _relic.Sprite;
        _tooltip.infoLeft = $"~{_relic.DisplayName}" +
            $"\n@{_relic.Description}";
        _tooltip.infoRight = "Relic";
    }
}