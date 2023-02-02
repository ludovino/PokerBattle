using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockPopup : MonoBehaviour
{
    [SerializeField]    
    private Image _sprite;
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _description;
    [SerializeField]
    private RectTransform _panel;
    [SerializeField]
    private Vector3 _startPosition;
    [SerializeField]
    private Vector3 _finalPosition;
    [SerializeField]
    private CanvasGroup _panelCanvasGroup;

    private CanvasGroup _backdropCanvasGroup;

    public void Awake()
    {
        _backdropCanvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeIn()
    {
        _panelCanvasGroup.alpha = 0;
        yield return _backdropCanvasGroup.DOFade(1, 0.3f).WaitForCompletion();
        _backdropCanvasGroup.blocksRaycasts = true;
        _backdropCanvasGroup.interactable = true;
    }
    public IEnumerator FadeOut()
    {
        _panelCanvasGroup.alpha = 0;
        _backdropCanvasGroup.alpha = 0;
        _backdropCanvasGroup.blocksRaycasts = false;
        _backdropCanvasGroup.interactable = false;
        yield return null;
    }

    public IEnumerator Pop(UnlockDisplay unlockDisplay)
    {
        _panel.anchoredPosition = _startPosition; 
        _panelCanvasGroup.DOFade(1, 0.3f);
        _sprite.sprite = unlockDisplay.Sprite;
        _name.text = unlockDisplay.Name;
        _description.text = unlockDisplay.Description;
        yield return _panel.DOAnchorPos(_finalPosition, 0.3f).SetEase(Ease.OutElastic).WaitForCompletion();
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        yield return Dismiss();
    }

    public IEnumerator Dismiss()
    {
        _panelCanvasGroup.DOFade(0, 0.3f);
        yield return _panel.DOAnchorPos(_startPosition, 0.3f).SetEase(Ease.InQuad).WaitForCompletion();
    }
}

