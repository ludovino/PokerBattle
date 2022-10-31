using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField]
    private AnimationCurve _returnCurve;
    [SerializeField]
    private float _returnTime;
    private DropTarget dropTarget;
    public void OnMouseDown()
    {
        if (!enabled) return;
        targetPosition = transform.position;
    }

    public void OnMouseDrag()
    {
        if (!enabled) return;
        var pos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        pos.z = 0;
        this.transform.position = pos;
    }

    public void OnMouseUp()
    {
        if (!enabled) return;
        if (dropTarget != null)
        {
            dropTarget.Drop(this);
        }
        else
        {
            Return();
        }
    }
    public void SetDropTarget(DropTarget target)
    {
        Debug.Log("SetDrop");
        dropTarget = target;
    }
    public void ClearDropTarget(DropTarget target)
    {
        if (dropTarget == target) dropTarget = null;
    }
    public void Return(){
        StartCoroutine(ReturnCr());
    }
    private IEnumerator ReturnCr(){
        float startTime = _returnCurve[0].time;
        float endTime = _returnCurve[_returnCurve.length - 1].time;
        float returnTimeRecip = 1f / _returnTime;
        Vector3 startPos = this.transform.position;
        for(float t = startTime; t < endTime; t += Time.deltaTime * returnTimeRecip)
        {
            var value = _returnCurve.Evaluate(t);
            this.transform.position = Vector3.LerpUnclamped(startPos, targetPosition, value);
            yield return null;
        }
        this.transform.position = targetPosition;
    }

    public void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
