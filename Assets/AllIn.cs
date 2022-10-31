using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AllIn : MonoBehaviour
{
    TextMeshPro _text;
    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
    }
    private void OnEnable()
    {
        CoroutineQueue.Defer(CR_AllIn());
    }

    private IEnumerator CR_AllIn()
    {
        var col = _text.color;
        col.a = 0;
        _text.color = col;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.4f));
        seq.Join(_text.DOFade(1, 0.2f));
        seq.Append(_text.DOFade(0, 0.2f));
        seq.OnStart(() => SfxManager.Gasp());
        seq.Play();
        yield return seq.WaitForCompletion();
        this.gameObject.SetActive(false);
    }
}
