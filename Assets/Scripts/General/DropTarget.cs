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

    public void Drop(GameObject draggable) => onDrop.Invoke(draggable);
    public class OnDrop : UnityEvent<GameObject>{}
}
