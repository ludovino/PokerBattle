using DG.Tweening;
using Mono.Cecil;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardFan : MonoBehaviour
{
    [SerializeField]
    private float _animationTime;
    
    [SerializeField]
    private float maxSpacing;
    [SerializeField]
    private List<GameObject> _cards;
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    [Min(0.001f)]
    private float width;
    private List<Tweener> _tweeners;
    private float spacing => Mathf.Min(maxSpacing, width / (_cards.Count + 1));
    private float endSpace => (width - spacing * (_cards.Count - 1)) / 2f;
    private void Awake()
    {
        _cards ??= new List<GameObject>();
        _tweeners ??= new List<Tweener>();
    }
    private void OnDrawGizmosSelected() 
    {
        Vector3 halfLength = new Vector3( width * 0.5f, 0, 0);
        Gizmos.DrawLine(transform.position - halfLength, transform.position + halfLength);
    }

    private void OnValidate() 
    {
        _cards ??= new List<GameObject>();
        _tweeners ??= new List<Tweener>();
        AlignHand();
    }

    public void AddToHand(GameObject card)
    {
        _cards.Add(card);
        AlignHand();
    }

    public void RemoveCard(GameObject card)
    {
        _cards.Remove(card);
        AlignHand();
    }

    private void AlignHand()
    {
        foreach (var tweener in _tweeners){
            tweener.Kill();
        }
        _tweeners.Clear();

        float distanceAlongLine = endSpace;
        for (int i = 0; i < _cards.Count; i++)
        {
            GameObject card = _cards[i];
            SetPosFromLinePosition(distanceAlongLine, card.transform);
            distanceAlongLine += spacing;
        }
    }

    public void ReturnCard(GameObject card)
    {
        var position = _cards.IndexOf(card);
        if (position == -1) return;
        var distAlongLine = endSpace + spacing * position;
        SetPosFromLinePosition(distAlongLine, card.transform);
    }

    private void SetPosFromLinePosition(float distanceAlongLine, Transform cardTransform)
    {
        Vector3 halfLength = new Vector3( width * 0.5f, 0, 0);
        Vector3 origin =  transform.position - halfLength;
        float height = curve.Evaluate(distanceAlongLine / width);
        var position = origin + new Vector3(distanceAlongLine, height * width, distanceAlongLine * -0.01f);
        
        var dY = height - curve.Evaluate((distanceAlongLine / width) -0.001f);
        var dX = 0.001f;
        var curveGradient = dY / dX;
        var angle = Mathf.Atan(curveGradient) * Mathf.Rad2Deg;
        
        // immediately set the z-position for smooth entry into the card fan;
        cardTransform.position = new Vector3(
            cardTransform.position.x, 
            cardTransform.position.y,
            position.z);
        _tweeners.Add(cardTransform.DOMove(position, _animationTime));
        _tweeners.Add(cardTransform.DORotate(new Vector3(0, 0, angle), _animationTime));
    }
}
