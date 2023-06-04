using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTesterScript : MonoBehaviour
{
    [SerializeField] private TweenerCanvasCardDisplay _card;
    [SerializeField]
    private Suit _suit1;
    [SerializeField]
    private Suit _suit2;
    [SerializeField]
    private Face _face1;
    [SerializeField]
    private Face _face2;
    [SerializeField]
    private Face _ace;

    [SerializeField]
    private int _value1;
    [SerializeField]
    private int _value2;

    [Button]
    public void BetweenSuits()
    {
        StartCoroutine(CR_BetweenSuits());
    }
    private IEnumerator CR_BetweenSuits()
    {
        _card.Animate(_value1, _suit1, null, 1f);
        yield return new WaitForSeconds(1.5f);
        _card.Animate(_value1, _suit2, null, 1f);
        yield return new WaitForSeconds(1.5f);
    }

    [Button]
    public void BetweenValues()
    {
        StartCoroutine(CR_BetweenValues());
    }

    private IEnumerator CR_BetweenValues()
    {
        _card.Animate(_value1, _suit1, null, 1f);
        yield return new WaitForSeconds(1.5f);
        _card.Animate(_value2, _suit1, null, 1f);
        yield return new WaitForSeconds(1.5f);
    }

    [Button]
    public void BetweenFaces()
    {
        StartCoroutine(CR_BetweenFaces());
    }

    public IEnumerator CR_BetweenFaces()
    {
        _card.Animate(0, _suit1, _face1, 1f);
        yield return new WaitForSeconds(1.5f);
        _card.Animate(0, _suit1, _face2, 1f);
        yield return new WaitForSeconds(1.5f);
        _card.Animate(_value1, _suit1, null, 1f);
        yield return new WaitForSeconds(1.5f);
        _card.Animate(0, _suit1, _face1, 1f);
        yield return new WaitForSeconds(1.5f);
    }
    [Button]
    public void NullSuitFace()
    {
        StartCoroutine(CR_NullSuitFace());
    }
    private IEnumerator CR_NullSuitFace()
    {
        _card.Animate(0, null, _face1, 1f);
        yield return new WaitForSeconds(1.5f);
        _card.Animate(_value1, _suit1, null, 1f);
        yield return new WaitForSeconds(1.5f);


        _card.Animate(_value1, null, null, 1f);
        yield return new WaitForSeconds(1.5f);
        _card.Animate(0, _suit1, _face1, 1f);
        yield return new WaitForSeconds(1.5f);
    }
}
