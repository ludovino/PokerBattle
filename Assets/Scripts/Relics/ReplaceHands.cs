using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceHands : Relic, IOnCollect
{
    [SerializeField]
    private List<Replacement> _replacements;

    public void OnCollect()
    {
        foreach(var replacement in _replacements)
        {
            PlayerData.Instance.HandList.Replace(replacement.toRemove, replacement.toAdd);
        }
    }
    [Serializable]
    public class Replacement
    {
        public PokerHand toAdd;
        public PokerHand toRemove;
    }
}
