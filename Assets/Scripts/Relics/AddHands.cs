using System;
using System.Collections.Generic;
using UnityEngine;

public class AddHands : Relic, IOnCollect
{
    [SerializeField]
    private List<PokerHand> _pokerHands;
    public void OnCollect()
    {
        PlayerData.Instance.HandList.AddHands(_pokerHands);
    }
}