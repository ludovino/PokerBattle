using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onEndTurn;
    [SerializeField]
    private UnityEvent _onStartTurn;
    [SerializeField]
    protected BattleController battle;
    public UnityEvent onEndTurn => _onEndTurn;
    public UnityEvent onStartTurn => _onStartTurn;

    public void Awake()
    {
        _onStartTurn = _onStartTurn ?? new UnityEvent();
        _onEndTurn = _onEndTurn ?? new UnityEvent();
    }
    public abstract void Init();
    public virtual void StartTurn()
    {
        Debug.Log($"{gameObject.name} start turn");
        enabled = true;
        _onStartTurn.Invoke();
    }

    public abstract void ChooseCards(List<Card> cards, int count, Action<List<Card>> selectCallback);

    public void EndTurn()
    {
        var result = battle.EndTurn();
        if (result)
        {
            _onEndTurn.Invoke();
            enabled = false;
        }
    }
}