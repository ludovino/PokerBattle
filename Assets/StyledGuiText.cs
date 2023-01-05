using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StyledGuiText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private SimpleTooltipStyle _style;
    private bool triggered = false;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.OnPreRenderText += _text_OnPreRenderText;
    }

    private void _text_OnPreRenderText(TMP_TextInfo obj)
    {
        if (triggered) 
        { 
            triggered = false;
            return; 
        };
        triggered = true;
        var textComponent = obj.textComponent;
        var text = textComponent.text;
        _style.Apply(ref text);
        _text.text = text;
        _text.ForceMeshUpdate(false, true);
    }
}
