using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "CardEffectList")]
public class CardEffectList : ScriptableObject
{
    [SerializeField]
    private List<SuitEffect> _initialCardEffects;
    private List<SuitEffect> _cardEffects;
    public void DoCardEffects<T>(ICard card, CardEffectContext context) where T : ICardEffect
    {
        var effects = _cardEffects.OfType<T>().Where(c => c.Condition(card));
        foreach (var effect in effects)
        {
            effect.Execute(context);
        }
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
        var descriptions = _initialCardEffects.Where(e => e.Suit == suit).Select(e => e.Description).ToList();
        return string.Join("\n", descriptions);
    }
}

