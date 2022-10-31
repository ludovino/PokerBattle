using UnityEngine;
public class CardSlot : MonoBehaviour
{
    public PlayerController playerController;
    public int slotNumber;
    private DropTarget _target;
    public void Awake()
    {
        _target = GetComponent<DropTarget>();
    }
    public void Start()
    {
        _target.onDrop.AddListener(OnDrop);
        playerController.onStartTurn.AddListener(EnableDropTarget);
        playerController.onEndTurn.AddListener(DisableDropTarget);
    }
    public void DisableDropTarget()
    {
        _target.Deactivate();
    }
    public void EnableDropTarget()
    {
        _target.Activate();
    }

    private void OnDrop(Draggable cardDraggable)
    {
        var card = cardDraggable.GetComponent<CardScript>();
        var result = playerController.Play(slotNumber, card);
        if(!result) cardDraggable.Return();
    }
}