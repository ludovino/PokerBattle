using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SuitToggle : MonoBehaviour
{
    private Suit _suit;
    public Suit Suit => _suit;
    [SerializeField]
    private bool _selected;
    [SerializeField]
    private Toggle _toggle;
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Image _checkmark;
    public bool Selected => _selected;
    public OnToggleSuit _onToggle;

    private void Awake()
    {
        _onToggle = _onToggle ?? new OnToggleSuit();
    }
    public void Init(bool selected, Suit suit, UnityAction<SuitToggle> onToggle)
    {
        Select(selected);
        _suit = suit;
        _background.sprite = _checkmark.sprite = _suit.displaySprite;
        _background.color = _suit.Color.Light;
        _checkmark.color = _suit.Color.Value;
        _toggle.onValueChanged.AddListener(Select);
        _onToggle.AddListener(onToggle);
    }

    public void Select(bool selected)
    {
        _toggle.isOn = _selected = selected;
        _onToggle.Invoke(this);
    }

    public void OnDestroy()
    {
        _toggle.onValueChanged.RemoveAllListeners();
    }

    public class OnToggleSuit : UnityEvent<SuitToggle> { }
}