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
    private CardFactory _cardFactory;
    public void Awake()
    {
        _cardFactory = Resources.Load<CardFactory>("CardFactory");
    }
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
        var cards = _cardFactory.GetCards(_hand.example);
        for(var i = 0; i < 5; i++)
        {
            _cardDisplays[i].UpdateCardDisplay(cards[i]);
        }
    }
}
