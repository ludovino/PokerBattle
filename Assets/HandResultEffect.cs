using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandResultEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _basicSystems;
    [SerializeField]
    private ParticleSystem _suitSystemPrefab;
    private List<ParticleSystem> _suitSystems;
    private List<ParticleSystem> _toPlay;
    private void Start()
    {
        _suitSystems = new List<ParticleSystem>();
        for(int i = 0; i < 5; i++)
        {
            var system = Instantiate(_suitSystemPrefab);
            system.transform.position = transform.position;
            _suitSystems.Add(system);
        }
    }

    public void Init(IEnumerable<CardScript> cardScripts)
    {
        _toPlay = new List<ParticleSystem>(_basicSystems);
        var suits = cardScripts.Select(c => c.card.suit).Where(s => s != null).ToList();
        if (!suits.Any()) return;
        var points = cardScripts.Sum(c => c.blackjackValue);
        var perSuit = points / suits.Count;
        for (int i = 0; i < suits.Count; i++)
        {
            Suit suit = suits[i];
            var system = _suitSystems[i];
            var em = system.emission;
            em.rateOverTime = perSuit;

            var tex = system.textureSheetAnimation;
            tex.SetSprite(0, suit.displaySprite);

            var main = system.main;
            main.startColor = suit.Color.Value;

            _toPlay.Add(system);
        }
    }

    public void Fire()
    {
        foreach(var system in _toPlay)
        {
            system.Play();
        }
    }
}
