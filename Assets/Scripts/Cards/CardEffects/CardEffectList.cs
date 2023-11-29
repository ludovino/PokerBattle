using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
[CreateAssetMenu(menuName = "Card/EffectList")]
public class CardEffectList : ScriptableObject
{
    [SerializeField]
    private List<CardEffect> _initialCardEffects;
    private List<CardEffect> _cardEffects;

    public IEnumerable<T> ForCard<T>(ICard card) where T : ICardEffect
    {
        return _cardEffects.Cast<ICardEffect>().OfType<T>().Where(c => c.Condition(card));
    }

    public void OnEnable()
    {
        _cardEffects = _initialCardEffects;
    }

    public string CardEffectDescription(ICard card)
    {
        var effects = _cardEffects.Where(c => c.Condition(card));
        var descriptions = effects.Select(e => e.Description).ToList();
        return string.Join("\n", descriptions);
    }

    public string SuitEffectDescription(Suit suit)
    {
        var descriptions = _initialCardEffects.OfType<SuitEffect>().Where(e => e.Suit == suit).Select(e => e.Description).ToList();
        return string.Join("\n", descriptions);
    }
}

