using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class CardPileIcon : MonoBehaviour
{
    [SerializeField]
    private int count;
    [SerializeField]
    private int max;
    [SerializeField] 
    private TextMeshProUGUI _countText;
    [SerializeField]
    private RectTransform _pileSpritePosition;
    [SerializeField]
    private float cardsPerPixel;
    
    public void SetPile(int cardCount)
    {
        var pixels = Mathf.FloorToInt(Mathf.Min(cardCount, max) / cardsPerPixel);
        _pileSpritePosition.anchoredPosition = new Vector2(-pixels, pixels);
        _countText.text = cardCount.ToString();
    }

    [Button]
    public void SetPile()
    {
        SetPile(count);
    }
}
