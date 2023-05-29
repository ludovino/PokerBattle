using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToMainCamera : MonoBehaviour
{
    private Canvas _canvas;
    [SortingLayer]
    [SerializeField]
    private string _sortingLayer;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }
    private void Start()
    {
        _canvas.worldCamera = Camera.main;
        _canvas.sortingLayerName = _sortingLayer;
    }
}
