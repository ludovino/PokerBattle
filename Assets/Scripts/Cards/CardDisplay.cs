using DG.Tweening;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private ICard _card;

    //TODO delete card display

    //private AnimancerComponent _animancer;
    [SerializeField]
    private TextMeshPro[] _numeralText;
    [SerializeField]
    private TextMeshProUGUI[] _canvasNumeralText;
    [SerializeField]
    private SpriteRenderer[] _spriteRenderers; 
    [SerializeField]
    private Image[] _canvasPips;
    [SerializeField]
    private TMP_FontAsset _regularFont; 
    [SerializeField]
    private TMP_FontAsset _narrowFont;
    [SerializeField]
    private AnimationClip[] _valueAnimations;
    [SerializeField]
    private SpriteRenderer _faceSprite;
    [SerializeField]
    private Image _canvasFace;
    [SerializeField]
    private AnimationClip[] _spareAnimations;
    private CardBackScript _cardBack;
    public bool FaceUp => _cardBack?.faceUp ?? true;
    private void Awake()
    {
        _cardBack = GetComponent<CardBackScript>();
    }
    public void UpdateCardDisplay(ICard card)
    {
        _card = card;
        UpdateCardDisplay();
    }

    private void OnValidate()
    {
        _valueAnimations = Resources.LoadAll<AnimationClip>("Animations/Card/Values").OrderBy(c => c.name).ToArray();
    }

    public void UpdateCardDisplay()
    {
        SetSprites();
        SetText();
        //if(isActiveAndEnabled) _animancer.Play(GetClip(_card), 0f);
    }

    private void OnEnable()
    {
        //if(_card != null) _animancer.Play(GetClip(_card), 0f);
    }
    public void AnimateCardDisplay(ICard card)
    {
        _card = card;
        CoroutineQueue.Defer(CR_AnimateCardDisplay(card));
    }
    public IEnumerator CR_AnimateCardDisplay(ICard card)
    {
        if (!FaceUp) SetSprites();
        else yield return AnimateSprites(card);
        SetText(card);
        //yield return _animancer.Play(GetClip(card), FaceUp ? 0.25f : 0f);
    }

    private AnimationClip GetClip(ICard card)
    {
        return card.face?.clip ?? _valueAnimations[Mathf.Clamp(card.highCardRank, 0, 21)];
    }

    public void SetSprites()
    {
        foreach(var sprite in _spriteRenderers)
        {
            sprite.sprite = _card.suit?.sprite;
            sprite.color = _card.suit?.Color.Value ?? Color.grey;
        }

        foreach(var image in _canvasPips)
        {
            image.sprite = _card.suit?.sprite;
            image.color = _card.suit?.Color.Value ?? Color.white;
        }

        if(_card.face != null)
        {
            var faceSprite = _card.face?.blankSprite;
            faceSprite = _card.suit?.Faces.GetValueOrDefault(_card.face) ?? faceSprite;
            if(_faceSprite) _faceSprite.sprite = faceSprite;
            if(_canvasFace) _canvasFace.sprite = faceSprite;
        }
    }
    public void SetText()
    {
        SetText(_card);
    }
    public void SetText(ICard card)
    {
        foreach(var text in _numeralText)
        {
            text.text = card.numeral;
            text.font = card.highCardRank >= 10 ? _narrowFont : _regularFont;
            text.color = card.suit?.Color.Value ?? Color.grey;
        }

        foreach (var text in _canvasNumeralText)
        {
            text.text = card.numeral;
            text.font = card.highCardRank >= 10 ? _narrowFont : _regularFont;
            text.color = card.suit?.Color.Value ?? Color.grey;
        }
    }
    public IEnumerator AnimateSprites(ICard card)
    {
        var seq = DOTween.Sequence();
        foreach (var sprite in _spriteRenderers)
        {
            seq.Insert(0, sprite.DOFade(0, 0.1f).OnComplete(() => sprite.sprite = card.suit?.sprite));
            seq.Insert(0.1f, sprite.DOColor(card.suit?.Color.Value ?? Color.grey, 0.1f));
        }

        if (card.face != null && _faceSprite)
        {
            var faceSprite = card.face?.blankSprite;
            faceSprite = card.suit?.Faces.GetValueOrDefault(card.face) ?? faceSprite;

            seq.Insert(0, _faceSprite.DOFade(0, 0.1f).OnComplete(() => _faceSprite.sprite = faceSprite));
            seq.Insert(0.1f, _faceSprite.DOFade(1, 0.1f));
        }
        if (card.face != null && _canvasFace)
        {
            var faceSprite = card.face?.blankSprite;
            faceSprite = card.suit?.Faces.GetValueOrDefault(card.face) ?? faceSprite;

            seq.Insert(0, _canvasFace.DOFade(0, 0.1f).OnComplete(() => _canvasFace.sprite = faceSprite));
            seq.Insert(0.1f, _canvasFace.DOFade(1, 0.1f));
        }
        seq.Play();
        yield return seq.WaitForCompletion();
    }
}
