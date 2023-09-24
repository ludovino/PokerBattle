using UnityEngine;

public class PlayableCardController : MonoBehaviour 
{
    [SerializeField] private Draggable draggable;
    [SerializeField] private Returner returner;
    private void Awake() {
        StateMachine _sm = new StateMachine();
        
    }

    private class Dragging : IState
    {
        public void OnEnter(){}

        public void OnExit(){}
    }

    private class Played : IState
    {
        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }

    private class Returning : IState
    {
        private Returner returner;
        public void OnEnter()
        {
            returner.Return();
        }

        public void OnExit()
        {
            returner.StopReturn();
        }
    }

    private class MouseOver : IState
    {
        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}