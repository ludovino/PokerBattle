using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    [SerializeField]
    private List<CardDisplayUI> _cardDisplays;
    [SerializeField]
    private TextMeshProUGUI _handName;
    [SerializeField]
    private PokerHand _hand;
    public void Start()
    {
        UpdateDisplays();
    }

    public void SetHand(PokerHand hand)
    {
        _hand = hand;
        UpdateDisplays();
    }

    public void UpdateDisplays()
    {
        _handName.text = _hand.DisplayName;
        var cards = CardFactory.Instance.GetCards(_hand.example);
        for(var i = 0; i < 5; i++)
        {
            _cardDisplays[i].UpdateCardDisplay(cards[i]);
        }
    }
}
