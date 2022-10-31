using UnityEngine;
using UnityEngine.Events;

public class DropTarget : MonoBehaviour
{
    public OnDrop onDrop;
    private Collider2D _col;
    public void Awake()
    {
        onDrop = onDrop ?? new OnDrop();
        _col = GetComponent<Collider2D>();
    }

    public void Activate()
    {
        this.enabled = true;
        _col.enabled = true;
    }
    public void Deactivate()
    {
        this.enabled = false;
        _col.enabled = false;
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("ENTER");
        var draggable = collider.GetComponent<Draggable>();
        draggable.SetDropTarget(this);
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        var draggable = collider.GetComponent<Draggable>();
        draggable.ClearDropTarget(this);
    }

    public void Drop(Draggable draggable) => onDrop.Invoke(draggable);
    public class OnDrop : UnityEvent<Draggable>{}
}
