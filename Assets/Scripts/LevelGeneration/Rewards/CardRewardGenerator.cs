using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Rewards/CardGenerator")]
internal class CardRewardGenerator : RewardGenerator
{
    [SerializeField]
    private CardPool _cardPool;
    [SerializeField]
    private CardSelectMenu _selectMenu;
    [SerializeField]
    private Sprite _sprite;
    public override IReward Generate()
    {

        List<Card> cards = _cardPool.GetWithoutReplacement(5, false);
        CardSelectMenu selectMenu = Instantiate(_selectMenu);
        selectMenu.Init(cards, 1);

        return new CardReward(cards, selectMenu, _sprite);
    }

    public class CardReward : IReward
    {
        private readonly UnityEvent _completed;
        private readonly List<Card> _cards;
        private readonly CardSelectMenu _selectMenu;
        private readonly Sprite _sprite;
        private bool _complete;

        public CardReward(List<Card> cards, CardSelectMenu _selectMenu, Sprite sprite)
        {
            _cards = cards;
            this._selectMenu = _selectMenu;
            _sprite = sprite;
            _complete = false;
            _completed ??= new UnityEvent();
        }

        public UnityEvent Completed => _completed;

        public Sprite Sprite => _sprite;

        public string Name => "Choose a card";

        public string Tooltip => "";

        public bool Complete => _complete;

        public void OpenReward()
        {
            _selectMenu.StartSelect();
            _selectMenu.OnSelect.AddListener(ChooseCard);
        }
        private void ChooseCard(List<CardScript> cards)
        {
            if(!cards.Any()) return;
            PlayerData.Instance.AddCard(cards.First().card);
            _complete = true;
            _completed.Invoke();
        }
    }
}

