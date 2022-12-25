using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EntityData")]
public class EntityData : ScriptableObject, IOnInit
{
    [TextArea(4, 10)]
    [SerializeField]
    private string _deckString;
    [SerializeField]
    protected int _chips;
    [SerializeField]
    protected int _drawBonus;
    [SerializeField]
    private HandList _handList;
    [SerializeField]
    protected CardPool _cardPool;
    private CardFactory _factory;
    protected List<Card> _cards;
    public int Chips => _currentChips;
    private int _currentChips;
    public int DrawBonus => _drawBonus;
    public HandList HandList => _handList;
    public IReadOnlyList<Card> Cards => _cards;
    public List<Card> CloneDeck => _cards.Select(c => c.Clone()).ToList();

    [SerializeField]
    private OnChangeChips _onChangeChips;
    private OnChangeDeck _onChangeDeck;
    public OnChangeChips OnChangeChips => _onChangeChips;
    public OnChangeDeck OnChangeDeck => _onChangeDeck;
    

    void OnValidate()
    {
        if (!Regex.IsMatch(_deckString.RemoveWhitespace(), @"(([A-Z]|\d{1,2})[A-Z\-];?\s?)+"))
            throw new System.FormatException($"{name} deck should be in the format 'AH;2H;5C;10S;JD;'");
    }


    void OnEnable()
    {
        _factory = Resources.Load<CardFactory>("CardFactory");
        Init();
    }

    public void Init()
    {
        _onChangeChips = _onChangeChips ?? new OnChangeChips();
        _onChangeDeck = _onChangeDeck ?? new OnChangeDeck();
        _currentChips = _chips;
        _cards = _factory.GetCards(_deckString);
        TriggerDeckChange();
    }
    public void AddCard(Card card)
    {
        _cards.Add(card);
        TriggerDeckChange();
    }

    public void TriggerDeckChange()
    {
        OnChangeDeck?.Invoke(Cards.OrderBy(c => c.highCardRank).ToList());
    }

    public void SetDeck(IEnumerable<Card> cards)
    {
        _cards = cards.ToList();
        TriggerDeckChange();
    }
    public void ChangeChips(int change)
    {
        var startingAmount = _currentChips;
        _currentChips += change;
        var currentAmount = _currentChips;
        OnChangeChips.Invoke(startingAmount, currentAmount, change);
    }
    public bool RemoveCard(Card card)
    {
        var toRemove = _cards.FirstOrDefault(c => c == card);
        if (toRemove == null) return false;
        _cards.Remove(toRemove);
        TriggerDeckChange();
        return true;
    }

}

public class OnChangeDeck : UnityEvent<IReadOnlyList<Card>> { }