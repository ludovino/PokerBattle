using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

public class FieldOfPlay : MonoBehaviour
{
    [SerializeField]
    private Entity _player;
    public CardScript[] cards;
    public Transform[] positions;
    void Start()
    {
        _player?.onPlay?.AddListener(OnPlay);
    }

    public void OnPlay(CardScript card, int slot)
    {
        CoroutineQueue.Defer(CR_OnPlay(card, positions[slot].position));
        card.Draggable = false;
    }

    public IEnumerator CR_OnPlay(CardScript card, Vector3 position)
    {
        card.transform.DORotate(Vector3.zero, 0.1f);
        yield return card.transform.DOMove(position, 0.1f).OnStart(() => SfxManager.PlayCard(2)).SetEase(Ease.OutQuart).WaitForCompletion();
    }

    void OnDestroy() 
    {
        _player?.onPlay?.RemoveListener(OnPlay);
    }
}
