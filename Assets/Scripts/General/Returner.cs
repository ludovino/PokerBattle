using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Returner : MonoBehaviour 
{
    private Vector3 returnPos;
    private Quaternion returnRotation;
    [SerializeField]
    private AnimationCurve _returnCurve;
    [SerializeField]
    private float _animateTime;
    private Tweener[] tweens = new Tweener[2];
    [SerializeField]
    [Foldout("events")]
    private UnityEvent returnStart;
    [SerializeField]
    [Foldout("events")]
    private UnityEvent returnEnd;
    
    public void Awake()
    {
        returnStart ??= new UnityEvent();
        returnEnd ??= new UnityEvent();
    }

    public void SetReturn(Vector3 position, Quaternion rotation)
    {
        returnPos = position;
        returnRotation = rotation;
    }
    
    public void Return()
    {
        returnStart.Invoke();
        if(!enabled) return;
        tweens[0] = transform.DOMove(returnPos, _animateTime).SetEase(_returnCurve).OnComplete(() => returnEnd.Invoke());
        tweens[1] = transform.DORotate(returnRotation.eulerAngles, _animateTime).SetEase(_returnCurve);
    }

    public void StopReturn()
    {
        foreach(var tweener in tweens)
        {
            tweener?.Kill();
        }
    }
}