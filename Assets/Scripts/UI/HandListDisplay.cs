using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HandListDisplay : MonoBehaviour
{
    [SerializeField]
    private HandDisplay _handDisplayPrefab;
    [SerializeField]
    private HandList _handList;
    [SerializeField]
    private RectTransform _listTransform;
    private List<HandDisplay> _handListItems;
    private void Awake()
    {
        _handListItems = _handListItems ?? new List<HandDisplay>();
    }

    private void Start()
    {
        UpdateListDisplay();
    }
    public void UpdateListDisplay()
    {
        SetListLength();
        var hands = _handList.Hands.OrderByDescending(h => h.rank).ToList();
        for (int i = 0; i < hands.Count; i++)
        {
            _handListItems[i].SetHand(hands[i]);
        }
    }

    public void SetListLength()
    {
        var toInstantiate = _handList.Hands.Count - _handListItems.Count;
        if(toInstantiate < 0) RemoveUnused(-toInstantiate);
        if(toInstantiate > 0) CreateNew(toInstantiate);
    }
    private void CreateNew(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var handDisplay = Instantiate(_handDisplayPrefab);
            handDisplay.GetComponent<RectTransform>().SetParent(_listTransform, false);
            _handListItems.Add(handDisplay);
        }
    }

    private void RemoveUnused(int numberToRemove)
    {
        var toRemove = _handListItems.Skip(_handListItems.Count - numberToRemove).ToList();
        for (var i = toRemove.Count - 1; i >= 0; i--)
        {
            _handListItems.Remove(toRemove[i]);
            Destroy(toRemove[i].gameObject);
        }
    }
}
