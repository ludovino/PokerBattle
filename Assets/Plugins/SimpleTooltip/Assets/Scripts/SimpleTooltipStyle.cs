using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

[Serializable]
[CreateAssetMenu]
public class SimpleTooltipStyle : ScriptableObject
{
    [System.Serializable]
    public struct Style
    {
        public string tag;
        public Color color;
        public bool bold;
        public bool italic;
        public bool underline;
        public bool strikethrough;
    }

    [Header("Tooltip Panel")]
    public Sprite slicedSprite;
    public Color color = Color.gray;

    [Header("Font")]
    public TMP_FontAsset fontAsset;
    public Color defaultColor = Color.white;

    [Header("Formatting")]
    public Style[] fontStyles;

    public void Apply(ref string text)
    {
        var styles = this.fontStyles;
        for (int i = 0; i < styles.Length; i++)
        {
            string addTags = "</b></i></u></s>";
            addTags += "<color=#" + ColorToHex(styles[i].color) + ">";
            if (styles[i].bold) addTags += "<b>";
            if (styles[i].italic) addTags += "<i>";
            if (styles[i].underline) addTags += "<u>";
            if (styles[i].strikethrough) addTags += "<s>";
            text = text.Replace(styles[i].tag, addTags);
        }
    }

    public void Apply(Image background)
    {
        background.sprite = slicedSprite;
        background.color = color;
    }

    public void Apply(TextMeshProUGUI tmp)
    {
        tmp.font = fontAsset;
        tmp.color = defaultColor;
        var text = tmp.text;
        Apply(ref text);
        tmp.text = text;
    }

    public static string ColorToHex(Color color)
    {
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        return r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
    }
}