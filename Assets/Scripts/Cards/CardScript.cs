using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CardScript : MonoBehaviour, ICard
{
    [SerializeField]
    private CardDisplay _cardDisplay;
    public UnityEvent onPlay;
    public UnityEvent onDraw;
    public UnityEvent onDiscard;
    private Card _card;
    private Card _tempCard;
    private Draggable _draggable;
    public bool Draggable { get { return _draggable.enabled; } set { _draggable.enabled = value; } }
    public void SetCard(Card card)
    {
        _card = card;
        _tempCard = _card.Clone();
        gameObject.name = card.ToString();
        _cardDisplay.UpdateCardDisplay(this);
    }
    public Card card => _tempCard ?? _card;
    public int highCardRank => card.highCardRank;
    public int blackjackValue => card.blackjackValue;
    public string numeral => card.numeral;
    public Suit suit => card.suit;
    public Face face => _card.face;

    void Awake()
    {
        onPlay = onPlay ?? new UnityEvent();
        onDraw = onDraw ?? new UnityEvent();
        onDiscard = onDiscard ?? new UnityEvent();
        _draggable = GetComponent<Draggable>();
    }
    public void Play()
    {
        onPlay.Invoke();
    }
    public void Draw()
    {
        onDraw.Invoke();
    }
    public void Discard()
    {
        onDiscard.Invoke();
    }

}
