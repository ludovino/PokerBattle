using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    [SerializeField]
    private Vector3 _cardRotation;
    public void OnDraw(List<CardScript> drawn)
    {
        var cards = drawn.ToList();
        CoroutineQueue.Defer(CR_OnDraw(cards));
    }

    private IEnumerator CR_OnDraw(List<CardScript> drawn)
    {
        foreach (var card in drawn)
        {
            card.gameObject.SetActive(true);
            card.transform.position = transform.position;
            card.transform.rotation = Quaternion.Euler(_cardRotation);
        }
        yield return null;
    }
}
