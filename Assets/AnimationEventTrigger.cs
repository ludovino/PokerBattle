using UnityEngine;
using UnityEngine.Events;

public class AnimationEventTrigger : MonoBehaviour
{
    public UnityEvent animationTrigger;
    private void Awake()
    {
        animationTrigger ??= new UnityEvent();
    }
    public void Trigger(){
        animationTrigger.Invoke();
    }
}
