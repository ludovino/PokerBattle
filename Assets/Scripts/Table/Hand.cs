using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private Entity _player;
    [SerializeField]
    private bool _playersHand;
    [SerializeField]
    private float _animationTime;
    private List<CardScript> _cards => _player.hand;
    public float spacing;
    // Start is called before the first frame update

    public void AddToHand(List<CardScript> cards)
    {
        List<CardScript> handCards = _cards.ToList();
        var drawn = cards.ToList();
        CoroutineQueue.Defer(CR_ArrangeHand(handCards, drawn));
    }

    public void AlignHand()
    {
        List<CardScript> handCards = _cards.ToList();
        CoroutineQueue.Defer(CR_ArrangeHand(handCards, new List<CardScript>()));
    }

    private IEnumerator CR_ArrangeHand(List<CardScript> handCards, List<CardScript> drawn)
    {
        var drawSequence = DOTween.Sequence();
        var offset = spacing * (handCards.Count + 1) / 2f;
        for (var i = 0; i < handCards.Count; i++)
        {
            var card = handCards[i];
            var offsetVector = new Vector3((i + 1) * spacing - offset, 0, -i * spacing);
            var position = transform.position + offsetVector;
            if (drawn.Contains(card))
            {
                drawSequence.Append(card.transform.DOMove(position, _animationTime).OnStart(() => SfxManager.PlayCard(0)));
                if (_playersHand) drawSequence.Join(card.transform.DORotate(Vector3.zero, _animationTime)).OnComplete(() => card.Draggable = true);
            }
            else drawSequence.Insert(0, card.transform.DOMove(position, _animationTime));
        }
        drawSequence.Play();
        yield return drawSequence.WaitForCompletion();
    }
}
