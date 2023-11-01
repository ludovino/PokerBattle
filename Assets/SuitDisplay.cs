using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO suit select rework

public class SuitDisplay : MonoBehaviour
{
    [SerializeField]
    private SimpleTooltipStyle _style;
    [SerializeField]
    private Image _suitImage;
    [SerializeField]
    private TextMeshProUGUI _text;
    private Suit _suit;
    [SerializeField]
    [TextArea(3,6)]
    private string _blankText;
    private Image _background;
    [SerializeField]
    private CardEffectList _effectList;

    private void Awake()
    {
        _background = GetComponent<Image>();
    }
    private void Start()
    {
    }
    public void SetDisplay(Suit suit)
    {
        if(suit == null)
        {
            SetDisplay();
            return;
        }
        _suit = suit;
        _suitImage.enabled = true;
        _suitImage.sprite = suit.sprite;
        _suitImage.color = suit.Color.Value;
        _text.text = $"~{suit.longName}" + "\n@" + _effectList.SuitEffectDescription(_suit);
        //_style.Apply(_text);
        //_style.Apply(_background);
    }

    private void SetDisplay()
    {
        _suit = null;
        _suitImage.enabled = false;
        _text.text = _blankText;
        //_style.Apply(_text);
        //_style.Apply(_background);
    }
}
