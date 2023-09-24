using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool dragging = false;
    private float dragStartTime;
    [SerializeField]
    private AnimationCurve _returnCurve;
    [SerializeField]
    private float _animateTime;
    private DropTarget dropTarget;
    
    [Foldout("events")]
    [SerializeField]
    private UnityEvent dragStart;
    [Foldout("events")]
    [SerializeField]
    private UnityEvent dragEnd;
    [Foldout("events")]
    [SerializeField]
    private UnityEvent drop;
    private Vector3 startPos;
    private Quaternion startRot;

    private void Awake() {
        dragStart ??= new UnityEvent();
        dragEnd ??= new UnityEvent();
        drop ??= new UnityEvent();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!enabled) return;
        dragStart.Invoke();
        dragStartTime = Time.time;
        dragging = true;
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!enabled) return;
        dragging = false;
        if (dropTarget != null)
        {
            drop.Invoke();
            dropTarget.Drop(this.gameObject);
        }
        else
        {
            dragEnd.Invoke();
        }
    }
    
    public void OnTriggerEnter2D(Collider2D collider)
    {
        var target = collider.attachedRigidbody.GetComponent<DropTarget>();
        dropTarget = target;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        var target = collider.attachedRigidbody.GetComponent<DropTarget>();
        if (dropTarget == target) dropTarget = null;
    }

    private void Update()
    {
        if (!dragging) return;
        
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        pos.z = 0;
        var time = (Time.time - dragStartTime) / _animateTime;

        //lerp to animate the card toward the mouse
        transform.position = Vector3.Lerp(startPos, pos, time);
        transform.rotation = Quaternion.Lerp(startRot, Quaternion.identity, time);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
    }
}
