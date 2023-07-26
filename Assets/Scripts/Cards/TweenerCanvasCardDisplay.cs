using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TweenerCanvasCardDisplay : MonoBehaviour
{
    [Foldout("Current Value")]
    [SerializeField]
    [Range(0, 21)]
    private int _value;
    [Foldout("Current Value")]
    [SerializeField]
    private Face _face;
    [Foldout("Current Value")]
    [SerializeField]
    private Suit _suit;

    [Foldout("Numeral Settings")]
    [SerializeField]
    private Image[] _numeralText;
    [Foldout("Numeral Settings")]
    [SerializeField]
    private Image[] _numeralPips;

    [Foldout("Pip Settings")]
    [SerializeField]
    private Image[] _pipSprites;
    [Foldout("Pip Settings")]
    [SerializeField]
    private List<NumeralSetup> _pipConfigs;

    [Foldout("Face Settings")]
    [SerializeField]
    private Image _faceSprite;
    [Foldout("Face Settings")]
    [SerializeField]
    private NumeralSetup _facePips;
    [Foldout("Face Settings")]
    [SerializeField]
    private Face _ace;

    [Foldout("Styles")]
    [SerializeField]
    private Color _blankColor;
    [Foldout("Styles")]
    [SerializeField]
    private TMP_FontAsset _regularFont;
    [Foldout("Styles")]
    [SerializeField]
    private TMP_FontAsset _narrowFont;

    private Queue<CardChange> _cardChanges;
    private CardBackScript _cardBack;

    private Dictionary<string, Sprite> _numeralSpritesByNameDict;
    [SerializeField]
    [Foldout("Styles")]
    private List<Sprite> _numerals;
    private Dictionary<string, Sprite> _numeralSpritesByName => _numeralSpritesByNameDict ?? SetNumeralSpriteDict();

    public bool FaceUp => _cardBack?.faceUp ?? true;

    private void Awake()
    {
        _cardChanges ??= new Queue<CardChange>();
        _cardBack = GetComponent<CardBackScript>(); 
        SetNumeralSpriteDict();
    }

    private Dictionary<string, Sprite> SetNumeralSpriteDict()
    {
        _numeralSpritesByNameDict = _numerals.ToDictionary(s => s.name, s => s);
        return _numeralSpritesByNameDict;
    }


    public void OnValidate()
    {
        Set(_value, _suit, _face, true);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _pipSprites.Length; i++)
        {
            if(i < _value && _face == null)
            {
                Handles.Label(_pipSprites[i].transform.position, (i + 1).ToString());
            }
        }
    }

    private string GetNumeralValueName(int value, Face face)
    {
        if(face != null) return face.numeral;
        return value.ToString("D2");
    }

    public void Set(ICard card, bool force)
    {
        Set(card.blackjackValue, card.suit, card.face, force);
    }
    public void Set(int value, Suit suit, Face face, bool force)
    {
        _cardChanges?.Clear();
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

    public IEnumerator CR_Animate(ICard card, float time)
    {
        Animate(card.blackjackValue, card.suit, card.face, time);
        yield return new WaitForSeconds(time);
    }

    public void Animate(ICard card, float time)
    {
        Animate(card.blackjackValue, card.suit, card.face, time);
    }

    public void Animate(int value, Suit suit, Face face, float time)
    {
        _value = value;
        _face = face;
        _suit = suit;

        var setup = _pipConfigs[value];
        if (face) setup = face.hasPips ? _facePips : _pipConfigs[0];
        
        PositionPips(setup, time);
        AnimateSuit(suit, setup, time);
        AnimateFace(face, time);
        AnimateNumeral(value, face, suit, time);
    }

    #region Numeral
    public void SetNumeral(int value, Face face, Suit suit)
    {
        var color = suit != null ? suit.Color.Value : _blankColor;
        var numeral = GetNumeralValueName(value, face);
        var sprite = _numeralSpritesByName[numeral];

        foreach (var numeralText in _numeralText)
        {
            numeralText.sprite = sprite;
            numeralText.color = color;
        }
    }

    public void AnimateNumeral(int value, Face face, Suit suit, float time)
    {
        var color = suit != null ? suit.Color.Value : _blankColor;
        var numeral = face?.numeral ?? value.ToString();

        foreach (var numeralText in _numeralText)
        {
            //numeralText.DOFade(0, 0.5f * time).OnComplete(() => numeralText.text = numeral);
            numeralText.DOColor(color , 0.5f * time).SetDelay(0.5f * time);
        }
    }
    #endregion

    #region Suit
    private void SetSuit(Suit suit)
    {
        _suit = suit;
        if (suit == null)
        {
            foreach (var pip in _pipSprites)
            {
                pip.sprite = null;
                pip.enabled = false;
                pip.color = Color.clear;
            }
            foreach (var smallpip in _numeralPips)
            {
                smallpip.sprite = null;
                smallpip.color = Color.clear;
            }
            return;
        }
        foreach(var pip in _pipSprites)
        {
            pip.sprite = suit.bigSprite;
            pip.color = Color.white;
        }
        foreach(var smallpip in _numeralPips)
        {
            smallpip.sprite = suit.smallSprite;
            smallpip.color = Color.white;
        }
    }  
    private void AnimateSuit(Suit suit, NumeralSetup numeralSetup, float time)
    {
        _suit = suit;
        var color = _suit?.Color.Value ?? Color.clear;
        for (int i = 0; i < _pipSprites.Length; i++)
        {
            Image pip = _pipSprites[i];
            if (i < numeralSetup.Pips.Count)
            {
                pip.DOFade(0, time * 0.5f).OnComplete(() =>
                {
                    pip.sprite = suit?.displaySprite;
                    pip.enabled = suit != null;
                });
                pip.DOColor(color, time * 0.5f).SetDelay(time * 0.5f);
            }
            else
            {
                pip.DOColor(Color.clear, time * 0.5f).OnComplete(() =>
                {
                    pip.sprite = suit?.displaySprite;
                    pip.enabled = false;
                });
            }
        }

        foreach (var smallpip in _numeralPips)
        {
            smallpip.DOFade(0, time * 0.5f).OnComplete(() => smallpip.sprite = suit != null ? suit.displaySprite : null);
            smallpip.DOColor(color, time * 0.5f).SetDelay(time * 0.5f);
        }
    }
    #endregion

    #region Face
    private void SetFace(Face face)
    {
        _face = face;
        if (face == null)
        {
            _faceSprite.color = new Color(1, 1, 1, 0);
            _faceSprite.enabled = false;
            return;
        }
        _faceSprite.color = new Color(1, 1, 1, 1);
        var pips = face.hasPips ? _facePips : _pipConfigs[0];
        _faceSprite.enabled = true;
        SetPips(pips);
        
        var faceSprite = _suit != null ? _suit.Faces[face] : face.blankSprite;

        _faceSprite.sprite = faceSprite;
    }
    private void AnimateFace(Face face, float time)
    {
        _face = face;
        if(face == null)
        {
            _faceSprite.DOFade(0, time).OnComplete(() => _faceSprite.enabled = false);
            return;
        }
        _faceSprite.enabled = true;

        var pips = face.hasPips ? _facePips : _pipConfigs[0];
        var faceSprite = _suit != null ? _suit.Faces[face] : face.blankSprite;
        var faceColor = Color.white;
        if(face == _ace && _suit != null)
        {
            faceSprite = _suit.displaySprite;
            faceColor = _suit.Color.Value;
        }

        _faceSprite.DOFade(0, time * 0.5f).OnComplete(() => _faceSprite.sprite = faceSprite);
        _faceSprite.DOColor(faceColor, time * 0.5f).SetDelay(time * 0.5f);
    }
    #endregion

    #region Pips
    private void SetPips(int value)
    {
        _value = value;
        if (_face) return; 
        var setup = _pipConfigs[value];
        SetPips(setup);
    }
    private void AnimatePips(int value, Face face, float time)
    {
        _value = value;
        var setup = _pipConfigs[value];
        if (face) setup = face.hasPips ? _facePips : _pipConfigs[0];
        PositionPips(setup, time);
    }
    private void SetPips(NumeralSetup setup)
    {
        var activePips = setup.Pips.Count;
        for (int i = 0; i < _pipSprites.Length; i++)
        {
            if (i < activePips)
            {
                _pipSprites[i].enabled = true;
                SetPip(_pipSprites[i].transform, setup.Pips[i]);
            }
            else
            {
                _pipSprites[i].enabled = false;
                _pipSprites[i].transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
    private void PositionPips(NumeralSetup setup, float time)
    {
        for (int i = 0; i < _pipSprites.Length; i++)
        {
            float animationTime = time - Random.Range(0, time * 0.2f);
            var sprite = _pipSprites[i];
            if (i < setup.Pips.Count)
            {
                AnimatePip(sprite.transform, setup.Pips[i], animationTime);
            }
            else
            {
                sprite.transform.DOLocalMove(Vector3.zero, 0).SetDelay(time);
                sprite.transform.DOLocalRotate(Vector3.zero, 0).SetDelay(time);
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
        pipTransform.DOLocalMove(new Vector3(pip.x, pip.y, 0), time).SetEase(Ease.InCubic);
        pipTransform.DOLocalRotate(new Vector3(0, 0, pip.z), time).SetEase(Ease.InCubic);
    }
    #endregion

    [Button]
    private void SetNumeralPipsConfig()
    {
        if (_face != null) return;
        var config = _pipConfigs[_value];
        for(int i = 0; i < _value; i++)
        {
            var pos = _pipSprites[i].transform.localPosition;
            var rot = _pipSprites[i].transform.localEulerAngles.z;
            config._pips[i] = new Vector3(pos.x, pos.y, rot);
        }
    }

    [Button]
    private void CopyPrevious()
    {
        if (_face != null || _value < 1) return;
        var config = _pipConfigs[_value];
        var prev = _pipConfigs[_value - 1];
        for (int i = 0; i < _value - 1; i++)
        {
            config._pips[i] = prev._pips[i];
        }

        Set(_value, _suit, _face, true);
    }

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

[System.Serializable]
public class NumeralSetup
{
    [SerializeField]
    public List<Vector3> _pips;
    public IReadOnlyList<Vector3> Pips => _pips;
}

