using System.Linq;
using UnityEngine;

public class BattleUiFunctions : MonoBehaviour
{
    [SerializeField]
    private BattleController _battleController;

    [SerializeField]
    private Entity _player;

    [SerializeField]
    private DeckDisplay _deckDisplay;

    public void ShowDrawPile()
    {
        _deckDisplay.gameObject.SetActive(true);
        _deckDisplay.SetCards(_player.DrawPile);
    }

    public void ShowDiscardPile()
    {
        _deckDisplay.gameObject.SetActive(true);
        _deckDisplay.SetCards(_player.DiscardPile);
    }
}
