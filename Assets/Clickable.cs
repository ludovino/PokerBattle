using System;
using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    [SerializeField]
    private Click _onClick;
    public Click OnClick { get => _onClick; }
    [SerializeField]
    private StartHover _onStartHover;
    public StartHover OnStartHover { get => _onStartHover; }
    [SerializeField]
    private EndHover _onEndHover;
    public EndHover OnEndHover { get => _onEndHover; }

    private void Awake()
    {
        _onClick = _onClick ?? new Click();
        _onStartHover= _onStartHover ?? new StartHover();
    }

    private void OnMouseUpAsButton()
    {
        _onClick.Invoke(this);
    }
    private void OnMouseEnter()
    {
        _onStartHover.Invoke(this);
    }
    private void OnMouseExit()
    {
        _onEndHover.Invoke(this);
    }

    [Serializable]
    public class Click : UnityEvent<Clickable>{ }
    [Serializable]
    public class StartHover : UnityEvent<Clickable> { }
    [Serializable]
    public class EndHover : UnityEvent<Clickable> { }

}