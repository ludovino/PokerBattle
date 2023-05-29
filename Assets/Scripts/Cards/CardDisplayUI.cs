using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayUI : MonoBehaviour
{

    private ICard _card;
    [SerializeField]
    private TextMeshProUGUI _numeralText;
    [SerializeField]
    private Image _suitSprite;
    [SerializeField]
    private TMP_FontAsset _regularFont;
    [SerializeField]
    private TMP_FontAsset _narrowFont;

    public void UpdateCardDisplay(ICard card)
    {
        _card = card;
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        _suitSprite.enabled = _card.suit != null;
        _suitSprite.sprite = _card.suit?.sprite;
        _numeralText.text = _card.numeral;
        _numeralText.font = _card.highCardRank >= 10 ? _narrowFont : _regularFont;
        var color = _card.suit?.Color.Value ?? Color.grey;
        _suitSprite.color = color;
        _numeralText.color = color;
    }
}
