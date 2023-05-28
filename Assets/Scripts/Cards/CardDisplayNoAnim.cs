using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayNoAnim : MonoBehaviour
{
    [SerializeField]
    private Image[] _numeralText;
    [SerializeField]
    private Image[] _pipSprites;
    [SerializeField]
    private Image[] _smallPips;
    [SerializeField]
    private Image _faceSprite;
    private CardBackScript _cardBack;

    [SerializeField]
    [Range(0, 21)]
    private int _value;
    [SerializeField]
    private Face _face;
    [SerializeField]
    private Suit _suit;

    [SerializeField]
    private List<NumeralSetup> _pipConfigs;

    [SerializeField]
    private NumeralSetup _facePips;

    [SerializeField]
    private Sprite[] _numeralSprites;

    [SerializeField]
    private Color _blankColor;

    private Queue<CardChange> _cardChanges;

    public bool FaceUp => _cardBack?.faceUp ?? true;

    private void Awake()
    {
        _cardChanges ??= new Queue<CardChange>();
        _cardBack = GetComponent<CardBackScript>();
    }

    public void OnValidate()
    {
        Set(_value, _suit, _face, true);
    }

    public void Set(int value, Suit suit, Face face, bool force)
    {
        var changeSuit = suit != _suit;
        var changeValue = value != _value;
        var changeFace = face != _face;

        if (changeSuit || force) SetSuit(suit);
        if(changeFace || (face && changeSuit) || force) SetFace(face);
        if(changeValue || changeSuit || force) SetNumeral(value, face, suit);
        if((changeValue && !face) || force) SetPips(value); 
        _value = value;
        _face = face;
        _suit = suit;
    }
    public void Animate(int value, Suit suit, Face face, float time)
    {
        var changeSuit = suit != _suit;
        var changeValue = value != _value;
        var changeFace = face != _face;

        if (changeSuit) AnimateSuit(suit, time);
        if(changeFace || (face && changeSuit)) AnimateFace(face, time);
        if(changeValue) AnimateNumeral(value, face, suit, time);
        if(changeValue && !face) AnimatePips(value, time);

        _value = value;
        _face = face;
        _suit = suit;
    }

    #region Numeral
    public void SetNumeral(int value, Face face, Suit suit)
    {
        var sprite = face != null ? face.numeralSprite : _numeralSprites[value];
        var color = suit != null ? suit.Color.Dark : _blankColor;

        foreach (var numeralText in _numeralText)
        {
            numeralText.sprite = sprite;
            numeralText.color = color;
        }
    }

    public void AnimateNumeral(int value, Face face, Suit suit, float time)
    {
        var sprite = face != null ? face.numeralSprite : _numeralSprites[value];
        var color = suit != null ? suit.Color.Dark : _blankColor;

        foreach (var numeralText in _numeralText)
        {
            numeralText.DOFade(0, 0.5f * time).OnComplete(() => numeralText.sprite = sprite);
            numeralText.DOColor(color , 0.5f * time).SetDelay(0.5f * time);
        }
    }
    #endregion

    #region Suit
    private void SetSuit(Suit suit)
    {
        foreach(var pip in _pipSprites)
        {
            pip.sprite = suit != null ? suit.sprite16 : null;
        }
        foreach(var smallpip in _smallPips)
        {
            smallpip.sprite = suit != null ? suit.sprite12 : null;
        }
    }  
    private void AnimateSuit(Suit suit, float time)
    {
        _suit = suit;
        foreach (var pip in _pipSprites)
        {
            var color = pip.color;
            pip.DOFade(0, time * 0.5f).OnComplete(() => pip.sprite = suit != null ? suit.sprite16 : null);
            pip.DOColor(color, time * 0.5f).SetDelay(time * 0.5f);
        }

        foreach (var smallpip in _smallPips)
        {
            smallpip.DOFade(0, time * 0.5f).OnComplete(() => smallpip.sprite = suit != null ? suit.sprite12 : null);
            smallpip.DOFade(1, time * 0.5f).SetDelay(time * 0.5f);
        }
    }
    #endregion

    #region Face
    private void SetFace(Face face)
    {
        if (face == null)
        {
            _faceSprite.color = new Color(1, 1, 1, 0);
            _faceSprite.enabled = false;
            return;
        }

        _faceSprite.enabled = true;
        _faceSprite.color = new Color(1, 1, 1, 1);
        if (face.pips) SetPips(_facePips);
        else SetPips(0);
        _faceSprite.sprite = _suit.Faces[face];
    }
    private void AnimateFace(Face face, float time)
    {
        if(face == null)
        {
            _faceSprite.DOFade(0, time).OnComplete(() => _faceSprite.enabled = false);
            return;
        }
        _faceSprite.enabled = true;
        if (face.pips) AnimatePips(_facePips, time);
        else AnimatePips(0, time);
        _faceSprite.DOFade(0, time * 0.5f).OnComplete(() => _faceSprite.sprite = _suit.Faces[face]);
        _faceSprite.DOFade(0, time * 0.5f).SetDelay(time * 0.5f);
    }
    #endregion

    #region Pips
    private void SetPips(int value)
    {
        if (_face) return;
        var setup = _pipConfigs[value];
        SetPips(setup);
    }
    private void AnimatePips(int value, float time)
    {
        if (_face) return;
        var setup = _pipConfigs[value];
        AnimatePips(setup, time);
    }
    private void SetPips(NumeralSetup setup)
    {
        var activePips = setup.Pips.Count;
        for (int i = 0; i < _pipSprites.Length; i++)
        {
            if (i < activePips)
            {
                _pipSprites[i].color = Color.white;
                SetPip(_pipSprites[i].transform, setup.Pips[i]);
            }
            else
            {
                _pipSprites[i].transform.localPosition = new Vector3(0, 0, 0);
                _pipSprites[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
    private void AnimatePips(NumeralSetup setup, float time)
    {
        for (int i = 0; i < _pipSprites.Length; i++)
        {
            if (i < setup.Pips.Count)
            {
                _pipSprites[i].DOFade(1f, time);
                AnimatePip(_pipSprites[i].transform, setup.Pips[i], time);
            }
            else
            {
                _pipSprites[i].DOFade(1f, time).OnComplete(() => _pipSprites[i].transform.localPosition = Vector3.zero);
            }
        }
    }
    private void SetPip(Transform pipTransform, Vector3 pip)
    {
        pipTransform.localPosition = new Vector3(pip.x, pip.y, 0);
        pipTransform.localRotation = Quaternion.Euler(0, 0, pip.z);
    }
    private void AnimatePip(Transform pipTransform, Vector3 pip, float time)
    {
        pipTransform.DOLocalMove(new Vector3(pip.x, pip.y, 0), time);
        pipTransform.DOLocalRotate(new Vector3(0, 0, pip.z), time);
    }
    #endregion

    private class CardChange
    {
        private readonly int _value;
        private readonly Suit _suit;
        private readonly Face _face;

        public CardChange(int value, Suit suit, Face face)
        {
            _value = value;
            _suit = suit;
            _face = face;
        }

        public int Value => _value;
        public Suit Suit => _suit;
        public Face Face => _face;
    }

    public void EnqueueChange(int value, Suit suit, Face face)
    {
        _cardChanges.Enqueue(new CardChange(value, suit, face));
    }

    public void TriggerNextChange(float time)
    {
        if(_cardChanges.TryDequeue(out var change))
        {
            Animate(change.Value, change.Suit, change.Face, time);
        }
    }
}

[Serializable]
public class NumeralSetup
{
    [SerializeField]
    public List<Vector3> _pips;
    public IReadOnlyList<Vector3> Pips => _pips;
}

