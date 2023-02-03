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
        yield return new WaitForSeconds(1f);
        SfxManager.Cheer();
        _chipPile.SetChips(0, player.Chips, 0);
    }
}
