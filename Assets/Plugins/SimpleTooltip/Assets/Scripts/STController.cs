﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class STController : MonoBehaviour
{
    public enum TextAlign { Left, Right };

    private Image panel;
    private TextMeshProUGUI toolTipTextLeft;
    private TextMeshProUGUI toolTipTextRight;
    private RectTransform rect;
    private int showInFrames = -1;
    private bool showNow = false;
    private Coroutine _showTooltip;
    
    private void Awake()
    {
        // Load up both text layers
        var tmps = GetComponentsInChildren<TextMeshProUGUI>();
        for(int i = 0; i < tmps.Length; i++)
        {
            if (tmps[i].name == "_left")
                toolTipTextLeft = tmps[i];

            if (tmps[i].name == "_right")
                toolTipTextRight = tmps[i];
        }

        // Keep a reference for the panel image and transform
        panel = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        // Hide at the start
        HideTooltip();
    }
    private void Update()
    {
        if (showNow) rect.anchoredPosition = Input.mousePosition;
    }
    private void ResizeToMatchText()
    {
        // Find the biggest height between both text layers
        var bounds = toolTipTextLeft.textBounds;
        float biggestY = toolTipTextLeft.textBounds.size.y;
        float rightY = toolTipTextRight.textBounds.size.y;
        if (rightY > biggestY)
            biggestY = rightY;

        // Dont forget to add the margins
        var margins = toolTipTextLeft.margin.y * 2;

        // Update the height of the tooltip panel
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, biggestY + margins);
    }
    public void SetRawText(string text, TextAlign align = TextAlign.Left)
    {
        // Doesn't change style, just the text
        if(align == TextAlign.Left)
            toolTipTextLeft.text = text;
        if (align == TextAlign.Right)
            toolTipTextRight.text = text;
        ResizeToMatchText();
    }

    public void SetCustomStyledText(string text, SimpleTooltipStyle style, TextAlign align = TextAlign.Left)
    {
        // Update the panel sprite and color
        style.Apply(panel);

        // Update the font asset, size and default color
        style.Apply(toolTipTextLeft); 
        style.Apply(toolTipTextRight);

        // Convert all tags to TMPro markup
        style.Apply(ref text);

        if (align == TextAlign.Left)
            toolTipTextLeft.text = text;
        if (align == TextAlign.Right)
            toolTipTextRight.text = text;
        ResizeToMatchText();
    }

    public void ShowTooltip(float delayInSeconds)
    {
        if(_showTooltip != null) StopCoroutine(_showTooltip);
        _showTooltip = StartCoroutine(CR_ShowTooltip(delayInSeconds));
    }


    private IEnumerator CR_ShowTooltip(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        if (this == null) yield break;
        ResizeToMatchText();
        showNow = true;
    }

    public void HideTooltip()
    {
        if (this == null) return;
        if (_showTooltip != null) StopCoroutine(_showTooltip);
        showInFrames = -1;
        showNow = false;
        rect.anchoredPosition = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
    }

    private void OnDestroy()
    {
        if (_showTooltip != null) StopCoroutine(_showTooltip);
    }
}