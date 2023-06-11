using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 targetPosition;
    [SerializeField]
    private AnimationCurve _returnCurve;
    [SerializeField]
    private float _returnTime;
    private DropTarget dropTarget;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!enabled) return;
        targetPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!enabled) return;
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        pos.z = 0;
        this.transform.position = pos;
    }
    public void OnEndDrag(PointerEventData eventData)
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
    //public void OnMouseDown()
    //{
    //}

    //public void OnMouseDrag()
    //{
    //}

    //public void OnMouseUp()
    //{
    //}
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
