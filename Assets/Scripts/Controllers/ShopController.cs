using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public Transform[] cardSlots;
    public TextMeshPro[] prices;
    public int removePrice;
    public TextMeshPro removePriceIndicator;
    public CardFactory cardFactory;
    public EntityData playerData;
    public CardSelector selectorPrefab;
    public Transform shopDeckOrigin;
    public Transform playerPile;
    private void Start()
    {
        var cards = cardFactory.GetRandomCards(cardSlots.Length).OrderByDescending(c => c.price).ToList();
        for (int i = 0; i < cardSlots.Length; i++)
        {
            Transform cardSlot = cardSlots[i];
            var card = cards[i];
            var selector = Instantiate(selectorPrefab);
            var priceText = cardSlot.GetComponentInChildren<TextMeshPro>();
            priceText.text = card.price.ToString();
            selector.SetCard(card);
            selector.transform.position = shopDeckOrigin.position;
            selector.transform.rotation = Quaternion.Euler(0, 180, 0);
            selector.transform.DOMove(cardSlot.position, 0.3f);
            selector.transform.DORotate(Vector3.zero, 0.3f);
            selector.OnSelectCard.AddListener(Buy);
        }
    }
    public void Buy(CardSelector cardSelector)
    {
        cardSelector.Deselect();
        var card = cardSelector.Card;
        if(playerData.Chips <= card.price) return;
        playerData.AddCard(card);
        playerData.ChangeChips(-card.price);
        cardSelector.transform.DOMove(playerPile.position, 0.3f).OnComplete(() => Destroy(cardSelector.gameObject));
        cardSelector.transform.DORotate(new Vector3(0, 180, 360), 0.3f, RotateMode.FastBeyond360);
    }

    public void Sell(CardSelector selectedCard)
    {
        var card = selectedCard.Card;
        if(playerData.Chips < card.price) return;
        playerData.RemoveCard(card);
        playerData.ChangeChips(-removePrice);
    }

    public void Exit()
    {
        GameController.Instance.GoToNextLevel();
    }
}
