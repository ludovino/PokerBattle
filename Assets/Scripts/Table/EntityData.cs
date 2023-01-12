using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Entities/EntityData")]
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
    CardEffectList _effectList;
    [SerializeField]
    protected CardPool _cardPool;
    protected List<Card> _cards;
    public int Chips => _currentChips;
    private int _currentChips;
    public int DrawBonus => _drawBonus;
    public HandList HandList => _handList;
    public CardEffectList EffectList => _effectList;
    public IReadOnlyList<Card> Cards => _cards;
    public List<Card> CloneDeck => _cards.Select(c => c.Clone()).ToList();

    [SerializeField]
    private OnChangeChips _onChangeChips;
    private OnChangeDeck _onChangeDeck;
    private OnAddRelic _onAddRelic;
    public OnChangeChips OnChangeChips => _onChangeChips;
    public OnChangeDeck OnChangeDeck => _onChangeDeck;
    public OnAddRelic OnAddRelic => _onAddRelic;

    private List<Relic> _relics;
    public IReadOnlyList<Relic> Relics => _relics;
    

    void OnValidate()
    {
        if (!Regex.IsMatch(_deckString.RemoveWhitespace(), @"(([A-Z]|\d{1,2})[A-Z\-];?\s?)+"))
            throw new System.FormatException($"{name} deck should be in the format 'AH;2H;5C;10S;JD;'");
    }


    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        _onChangeChips ??= new OnChangeChips();
        _onChangeDeck ??= new OnChangeDeck();
        _onAddRelic ??= new OnAddRelic();
        _currentChips = _chips;
        _cards = CardFactory.Instance.GetCards(_deckString);

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

    internal void AddRelic(Relic relic)
    {
        _relics.Add(relic);
        if (relic is IOnCollect r) r.OnCollect();
    }

}

public class OnChangeDeck : UnityEvent<IReadOnlyList<Card>> { }
public class OnAddRelic : UnityEvent<IReadOnlyList<Relic>> { }