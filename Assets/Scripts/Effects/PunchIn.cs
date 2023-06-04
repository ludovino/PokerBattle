using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchIn : MonoBehaviour
{
    private Vector3 _startingScale;
    [SerializeField]
    [Range(0f, 1f)]
    private float _strength = 0.5f;
    [SerializeField]
    [Range(0f, 1f)]
    private float _duration = 0.4f;
    [SerializeField]
    [Range(0f, 1f)]
    private float _elasticity = 1f;
    [SerializeField]
    [Range(0, 100)]
    private int _vibrato = 10;
    private Tween _tween;
    private void OnEnable()
    {
        _startingScale = transform.localScale;
        _tween = transform.DOPunchScale(Vector3.one * _strength, _duration, _vibrato, _elasticity);
    }

    private void OnDisable()
    {
        if (_tween.active)
        {
            _tween.Kill();
            transform.localScale = _startingScale;
        }
    }
}
