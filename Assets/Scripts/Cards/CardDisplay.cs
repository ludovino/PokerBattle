using Animancer;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    private ICard _card;
    [SerializeField]
    private AnimancerComponent _animancer;
    [SerializeField]
    private TextMeshPro[] _numeralText;
    [SerializeField]
    private SpriteRenderer[] _spriteRenderers;
    [SerializeField]
    private TMP_FontAsset _regularFont; 
    [SerializeField]
    private TMP_FontAsset _narrowFont;
    [SerializeField]
    private AnimationClip[] _valueAnimations;
    [SerializeField]
    private SpriteRenderer _faceSprite;
    [SerializeField]
    private SpriteRenderer _glow;
    [SerializeField]
    private AnimationClip[] _spareAnimations;
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
        if(isActiveAndEnabled) _animancer.Play(GetClip(), 0f);
        GlowOff();
    }
    public void SetAnimationState()
    {

    }

    public void Glow(float intensity, Color color)
    {
        if (intensity == 0)
        {
            GlowOff();
            return;
        }
        _glow.enabled = true;
        var targetColor = new Color(color.r, color.g, color.b, intensity);
        _glow.DOColor(targetColor, 0.2f);
        _glow.transform.DOScale(Vector3.one, 0.2f);
    }

    public void GlowOff()
    {
        _glow.DOColor(new Color(0, 0, 0, 0), 0.2f);
        _glow.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f).OnComplete(() => _glow.enabled = false);
    }

    private void OnEnable()
    {
        if(_card != null) _animancer.Play(GetClip(), 0f);
    }
    public void AnimateCardDisplay(ICard card)
    {
        _card = card;
    }
    public void AnimateCardDisplay()
    {
        SetSprites();
        SetText();
        _animancer.Play(GetClip(), 0.25f);
    }

    private AnimationClip GetClip()
    {
        return _card.face?.clip ?? _valueAnimations[Mathf.Clamp(_card.highCardRank, 0, 21)];
    }

    public void SetSprites()
    {
        foreach(var sprite in _spriteRenderers)
        {
            sprite.sprite = _card.suit?.sprite;
            sprite.color = _card.suit?.Color.Value ?? Color.grey;
        }
        if(_card.face != null)
        {
            var faceSprite = _card.face?.blankSprite;
            faceSprite = _card.suit?.Faces.GetValueOrDefault(_card.face) ?? faceSprite;
            _faceSprite.sprite = faceSprite;
        }
    }

    public void SetText()
    {
        foreach(var text in _numeralText)
        {
            text.text = _card.numeral;
            text.font = _card.highCardRank >= 10 ? _narrowFont : _regularFont;
            text.color = _card.suit?.Color.Value ?? Color.grey;
        }
    }
}
