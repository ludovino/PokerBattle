using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static HandResultDisplay;

public class VictoryScreen : MonoBehaviour
{
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
    void Start()
    {
        CoroutineQueue.Defer(CR_DoCards());
    }

    private IEnumerator CR_DoCards()
    {
        var sequence = DOTween.Sequence();
        var cards = player.Cards;
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            var display = Instantiate(_cardDisplayPrefab);
            display.UpdateCardDisplay(card);
            display.transform.position = _drawPile.position;
            display.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            var mov = display.transform.DOMove(_line.transform.position + new Vector3(i * spacing, 0, -i * spacing), 0.3f).SetEase(Ease.InOutCubic).OnStart(() => SfxManager.PlayCard(6));
            var rot = display.transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.InOutSine);
            sequence.Insert(0.1f * i, mov);
            sequence.Insert(0.1f * i, rot);
        }
        sequence.OnComplete(() => SfxManager.Cheer());
        sequence.Play();
        yield return sequence.WaitForCompletion();
        _chipPile.SetChips(0, player.Chips, 0);
    }
}
