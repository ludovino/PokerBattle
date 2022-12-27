using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : EntityController
{
    public Entity enemy;
    public Entity player;

    private List<Suit> _flushSuits;
    private List<Card> _straightCards;
    [SerializeField]
    private Straight _straight;
    private int _cardsPlayed;

    public override void Init()
    {
        var deck = enemy.entityData.CloneDeck;
        _flushSuits = deck.Where(c => c.suit != null).GroupBy(c => c.suit).Where(g => g.Count() >= 5).Select(g => g.Key).ToList();
        _straightCards = _straight.GetStraightCards(deck);
    }

    public override void StartTurn()
    {
        base.StartTurn();
        StartCoroutine(CR_turn());
    }

    public IEnumerator CR_turn()
    {
        _cardsPlayed = 0;
        var numberToPlay = player.slotsRemaining == 0 || enemy.chips == 0 ? enemy.slotsRemaining : 1;

        // opening hand
        if (enemy.slotsRemaining == 5)
        {
            var rankedHand = enemy.Evaluate(enemy.hand);
            if (rankedHand.rank >= 30)
            {
                Play(rankedHand.cards);
                yield return new WaitForEndOfFrame();
                EndTurn();
                yield break;
            }
            var toPlay = rankedHand.cards.Take(1).ToList();
            if (rankedHand.cards[1].highCardRank >= 10) toPlay.Add(rankedHand.cards[1]);
            Play(toPlay);
        }

        yield return new WaitForEndOfFrame();

        // fish for sequences and flushes

        var sequential = _straight.HasStraight(enemy.played.Cast<ICard>().AsReadOnlyCollection(), enemy.played.Count);
        yield return new WaitForEndOfFrame();

        var flushSuit = enemy.played.Suited() ? enemy.played.First().suit : null;
        yield return new WaitForEndOfFrame();

        if (sequential && enemy.played.All(c => _straightCards.Contains(c)))
        {
            var nextInSequence = _straight.NextSequentialValues(enemy.played.Cast<ICard>().ToList());
            var toPlay = enemy.hand.Where(c => nextInSequence.Contains(c.card));
            Play(toPlay);
        }

        yield return new WaitForEndOfFrame();
        if (flushSuit != null && _flushSuits.Contains(flushSuit))
        {
            var toPlay = enemy.hand.Where(c => c.card.suit == flushSuit).ToList();
            if (toPlay.Count > 0) Play(toPlay);
        }

        yield return new WaitForEndOfFrame();
        // fallback to pairs
        if (_cardsPlayed == 0 || flushSuit == null && !sequential)
        {
            var rankValues = enemy.played.Select(c => c.highCardRank).ToList();
            var toPlay = enemy.hand.Where(c => rankValues.Contains(c.highCardRank));
            Play(toPlay);
        }

        yield return new WaitForEndOfFrame();
        // play high cards if all else fails

        if (_cardsPlayed < numberToPlay)
        {
            Play(enemy.hand.OrderByDescending(c => c.highCardRank).Take(numberToPlay));
        }


        yield return new WaitForEndOfFrame();
        EndTurn();
    }
    private void Play(IEnumerable<CardScript> cards)
    {
        var toPlay = cards.Take(enemy.slotsRemaining).ToList();
        for (int i = 0; i < toPlay.Count; i++)
        {
            var card = toPlay[i];
            var slot = Array.IndexOf(enemy.fieldOfPlay, null);
            battle.Play(slot, card);
            _cardsPlayed++;
        }
    }

    public override void ChooseCards(List<Card> cards, int count, Action<List<Card>> selectCallback)
    {
        throw new NotImplementedException();
    }
}

