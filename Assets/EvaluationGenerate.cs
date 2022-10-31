using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvaluationGenerate : MonoBehaviour
{
    [SerializeField]
    CardFactory _cardFactory;
    [SerializeField]
    PokerHand _playerHand; 
    [SerializeField]
    Transform _playerHandPosition;
    [SerializeField]
    PokerHand _enemyHand;
    [SerializeField]
    Transform _enemyHandPosition;
    [SerializeField]
    CardScript _cardPrefab;

    [SerializeField]
    OnEvaluate _onEvaluate;

    private void Awake()
    {
        _onEvaluate = _onEvaluate ?? new OnEvaluate();
    }

    public Evaluation GetEvaluation()
    {
        var playerCards = GetCardInstances(_playerHand, _playerHandPosition.position);
        var enemyCards = GetCardInstances(_enemyHand, _enemyHandPosition.position);
        var playerHand = _playerHand.GetRankedHand(playerCards);
        var enemyHand = _enemyHand.GetRankedHand(enemyCards);

        var evaluation = new Evaluation(playerHand,enemyHand);

        return evaluation;
    }

    public void Generate()
    {
        var eval = GetEvaluation();
        _onEvaluate.Invoke(eval);
    }

    public List<CardScript> GetCardInstances(PokerHand hand, Vector3 position)
    {
        var cards = _cardFactory.GetCards(hand.example);
        var cardScripts = new List<CardScript>();
        var positions = Enumerable.Range(-2, 5).ToList();
        positions.Shuffle();
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            var cardScript = Instantiate(_cardPrefab);
            cardScript.SetCard(card);
            cardScript.transform.position = position + new Vector3(positions[i], 0, 0);
            cardScript.gameObject.SetActive(true);
            cardScripts.Add(cardScript);
        }
        return cardScripts;
    }
}