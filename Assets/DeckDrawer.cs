using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckDrawer : MonoBehaviour
{
    [SerializeField]
    private CardFactory _cardFactory;
    [SerializeField]
    private EntityData player;
    [SerializeField]
    private CardDisplay _cardDisplayPrefab;
    [SerializeField]
    private Transform _drawPile;
    [SerializeField]
    private float spacing;
    [SerializeField]
    private Transform _line;
    [SerializeField]
    private ChipPile _chipPile;
    private List<CardDisplay> _displays;
    private List<Card> _cards;

    [SerializeField]
    private Button[] _buttons;
    void Awake()
    {
        _displays = new List<CardDisplay>();
        _cards = new List<Card>();
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        DisableButtons();
        yield return new WaitForSeconds(0.7f);
        _chipPile.SetChips(0, player.Chips, 0);
        CoroutineQueue.Defer(DrawDeck());
    }

    private IEnumerator DrawDeck()
    {
        _displays = new List<CardDisplay>();
        DisableButtons();
        var sequence = DOTween.Sequence();
        var cards = _cardFactory.GetStartingDeck().OrderBy(c => c.highCardRank).ToList();
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            var display = Instantiate(_cardDisplayPrefab);
            display.UpdateCardDisplay(card);
            display.transform.position = _drawPile.position;
            display.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            var mov = display.transform.DOMove(_line.transform.position + new Vector3(i* spacing, 0, -i * spacing), 0.3f).SetEase(Ease.InOutCubic).OnStart(() => SfxManager.PlayCard(6));
            var rot = display.transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.InOutSine);
            sequence.Insert(0.1f * i, mov);
            sequence.Insert(0.1f * i, rot);
            _displays.Add(display);
            _cards.Add(card);
        }
        sequence.Play();
        yield return sequence.WaitForCompletion();
        EnableButtons();
    }

    public void Redraw()
    {
        for (int i = _displays.Count - 1; i >=0 ; i--)
        {
            CardDisplay card = _displays[i];
            Destroy(card.gameObject);
        }
        _displays.Clear();
        _cards.Clear();
        player.ChangeChips(-5);
        _chipPile.SetChips(0, player.Chips, 0);
        CoroutineQueue.Defer(DrawDeck());
    }

    public void Go()
    {
        player.SetDeck(_cards);
        GameController.Instance.DeckChosen();
    }

    private void EnableButtons()
    {
       foreach(var button in _buttons)
        {
            button.enabled = true;
        }
    }

    private void DisableButtons()
    {
        foreach (var button in _buttons)
        {
            button.enabled = false;
        }
    }
}
