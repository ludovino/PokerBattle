using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState currentState { 
        get => _currentState; 
        private set 
        {
            _currentState = value;
            _currentStateType = _currentState.GetType();
        }
    }


    private IState _currentState;
    private Type _currentStateType;
    private Action _onFail;
    private Dictionary<Type, HashSet<Type>> _transitions;
    private HashSet<Type> _fromAny;
    private HashSet<Type> _toAny;
    private bool _throwOnFail;

    public StateMachine(IState initialState, Action onFail = null, bool throwOnFail = true)
    {
        currentState = initialState;
        currentState.OnEnter();
        _transitions = new Dictionary<Type, HashSet<Type>>();
        _onFail = onFail;
        _fromAny = new HashSet<Type>();
        _toAny = new HashSet<Type>();
        _throwOnFail = throwOnFail;
    }
    public StateMachine()
    {
        _transitions = new Dictionary<Type, HashSet<Type>>();
        _onFail = null;
        _fromAny = new HashSet<Type>();
        _toAny = new HashSet<Type>();
        _throwOnFail = true;
    }
    public void Init(IState initialState)
    {
        currentState = initialState;
        currentState.OnEnter();
    }
    public void RegisterTransition<Tfrom, Tto>() 
    where Tfrom : IState 
    where Tto : IState 
    {
        var from = typeof(Tfrom);
        var to = typeof(Tto);
        if(_transitions.TryGetValue(from, out var toList)) 
        {
            toList.Add(to);
        } 
        else
        {
            _transitions.Add(from, new HashSet<Type>() {to});
        }
    }

    public void RegisterTransitionFromAny<Tto>()
    {
        _fromAny.Add(typeof(Tto));
    }

    public void RegisterTransitionToAny<Tfrom>()
    {
        _toAny.Add(typeof(Tfrom));
    }

    public void MoveToState(IState state)
    {
        if(!CanMove(state)){
            if(_throwOnFail) throw new Exception($"cannot move from state {_currentStateType.Name} to {state.GetType().Name}");
            if(_onFail != null) _onFail();  
            return;
        }
        if(currentState != null){
            currentState.OnExit();
            //Debug.Log($"Leaving State { currentState.GetType().Name }");
        }
        currentState = state;
        currentState.OnEnter();
        //Debug.Log($"Entering State { currentState.GetType().Name }");
    }

    private bool CanMove(IState state)
    {
        var tFrom = _currentStateType;
        var tTo = state.GetType();

        if(_toAny.Contains(tFrom)) return true;
        if(_fromAny.Contains(tTo)) return true;

        if(state == null) return false;
        if(_transitions.TryGetValue(tFrom, out var typeList))
        {
            return typeList.Contains(tTo);
        }
        return false;
    }

    public void Tick(){
        if(currentState is IOnUpdate s) s.Tick();
    }

    public void FixedTick()
    {
        if(currentState is IOnFixedUpdate s) s.FixedTick();
    }
}

public interface IState {
    void OnEnter();
    void OnExit();
}

public interface IOnFixedUpdate {
    void FixedTick();
}

public interface IOnUpdate {
    void Tick();
}
