using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Relic/HandAdd")]
public class AddHands : Relic, IOnCollect
{
    [SerializeField]
    private List<PokerHand> _pokerHands;
    public void OnCollect()
    {
        PlayerData.Instance.HandList.AddHands(_pokerHands);
    }
}