using System;
using UnityEngine;
public class CardSlot : MonoBehaviour
{
    public int SlotNumber { get; set; }
    private DropTarget _target;
    public event Func<CardSlot, CardScript, bool> OnCardDrop;
    public void Awake()
    {
        _target = GetComponent<DropTarget>();
    }
    public void Start()
    {
        _target.onDrop.AddListener(OnDrop);
    }
    public void DisableDropTarget()
    {
        _target.Deactivate();
    }
    public void EnableDropTarget()
    {
        _target.Activate();
    }

    private void OnDrop(GameObject cardGameObject)
    {
        var card = cardGameObject.GetComponent<CardScript>();
        var result = OnCardDrop(this, card);
    }
}