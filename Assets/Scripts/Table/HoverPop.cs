using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverPop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float distance;
    private Tweener tweener;
    private Returner returner;
    public bool hoverable;
    private bool hovering;
    private void Awake() 
    {
        returner = GetComponent<Returner>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        if(!hoverable) return;
        Pop();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        if(!hoverable) return;
        if(tweener != null) tweener.Kill();
        returner.Return();
    }
    public void SetHoverable(bool toSet)
    {
        if(hoverable == toSet) return;
        if(toSet && hovering) Pop();
        if(!toSet && tweener != null) tweener.Kill();
        hoverable = toSet;
    }

    private void Pop()
    {
        Debug.Log("POP");
        tweener = transform.DOMove(transform.position + (transform.up * distance), 0.3f).SetEase(Ease.OutQuad);
    }
}