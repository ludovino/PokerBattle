using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    [SerializeField]
    private Entity _player;
    void Start(){
        _player.onDiscard.AddListener(OnDiscard);
    }

    void OnDiscard(List<CardScript> cards)
    {
        var discarded = cards.ToList();
        CoroutineQueue.Defer(CR_Discard(discarded));
    }

    private IEnumerator CR_Discard(List<CardScript> cards)
    {
        var sequence = DOTween.Sequence();
        foreach (var card in cards)
        {
            var pos = card.transform.DOMove(transform.position, 0.15f).SetEase(Ease.OutQuad).OnStart(() => SfxManager.PlayCard(1)).OnComplete(() => card.gameObject.SetActive(false));
            var rot = card.transform.DORotate(Vector3.zero, 0.15f);
            var sca = card.transform.DOScale(Vector3.one, 0.15f);
            sequence.Append(pos).Join(rot).Join(sca);
        }
        sequence.Play();
        yield return sequence.WaitForCompletion();
    }
}
