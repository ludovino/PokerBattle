using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static HandResultDisplay;

public class CardCollection : MonoBehaviour
{
    [SerializeField]
    private float _addTime;
    [SerializeField]
    private float _addInterval;
    [SerializeField]
    private float _xOffset;
    [SerializeField]
    private float _zOffset;
    [SerializeField]
    private float _scale;
    [SerializeField]
    private bool _faceDown;
    [SerializeField]
    private int _addRotations;
    [SerializeField]
    private int _cardSfxClip;
    private List<GameObject> _cards;
    public void Awake()
    {
        _cards = new List<GameObject>();
    }

    public void AddCardsImmediate(IReadOnlyList<GameObject> cards)
    {
        var currentIndex = _cards.Count;
        _cards.AddRange(cards);
        for (int i = currentIndex; i < _cards.Count; i++)
        {
            GameObject card = _cards[i];
            card.transform.position = transform.position + new Vector3(i * _xOffset, 0, i * _zOffset);
            var yRotation = _faceDown ? 180f : 0f;
            card.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            card.transform.localScale = Vector3.one * _scale;
        }
    }

    public IEnumerator RemoveCards(IReadOnlyList<GameObject> cards)
    {
        foreach(var card in cards)
        {
            _cards.Remove(card);
        }
        var sequence = DOTween.Sequence();
        for (int i = 0; i < _cards.Count; i++)
        {
            GameObject card = _cards[i];
            var mov = card.transform.DOMove(transform.position + new Vector3(i * _xOffset, 0, i * _zOffset), 0.3f).SetEase(Ease.InOutCubic);
            var yRotation = _faceDown ? 180f : 0f;
            var rot = card.transform.DORotate(new Vector3(0, yRotation, 0), 0.3f).SetEase(Ease.InOutSine);
            var sca = card.transform.DOScale(Vector3.one * _scale, 0.3f).SetEase(Ease.InOutSine);
            var startTime = 0;
            sequence.Insert(startTime, mov);
            sequence.Insert(startTime, rot);
            sequence.Insert(startTime, sca);
        }
        sequence.Play();
        yield return sequence.WaitForCompletion();
    }

    public void RemoveCardsImmediate(IReadOnlyList<GameObject> cards)
    {
        foreach (var card in cards)
        {
            _cards.Remove(card);
        }

        for (int i = 0; i < _cards.Count; i++)
        {
            GameObject card = _cards[i];
            card.transform.position = transform.position + new Vector3(i * _xOffset, 0, i * _zOffset);
            var yRotation = _faceDown ? 180f : 0f;
            card.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            card.transform.localScale = Vector3.one * _scale;
        }
    }

    public IEnumerator AddCards(IReadOnlyList<GameObject> cards)
    {
        var currentIndex = _cards.Count;
        _cards.AddRange(cards.OrderBy(c => -c.transform.position.z));
        var sequence = DOTween.Sequence();
        for (int i = currentIndex; i < _cards.Count; i++)
        {
            GameObject card = _cards[i];
            var mov = card.transform.DOMove(transform.position + new Vector3(i * _xOffset, 0, i * _zOffset), 0.3f).SetEase(Ease.InOutCubic).OnStart(() => SfxManager.PlayCard(_cardSfxClip));
            var yRotation = _faceDown ? 180f : 0f;
            var rot = card.transform.DORotate(new Vector3(0,yRotation, 360 * _addRotations), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
            var sca = card.transform.DOScale(Vector3.one * _scale, 0.3f).SetEase(Ease.InOutSine);
            var startTime = _addInterval * (i - currentIndex);
            sequence.Insert(startTime, mov);
            sequence.Insert(startTime, rot);
            sequence.Insert(startTime, sca);
        }
        sequence.Play();
        yield return sequence.WaitForCompletion();
    }
}
