using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Relic/HandReplace")]
public class ReplaceHands : Relic, IOnCollect, IOnChangeHandList
{
    [SerializeField]
    private List<Replacement> _replacements;

    public void OnChangeHandList()
    {
        Replace();
    }

    public void OnCollect()
    {
        Replace();
    }

    
    private void Replace()
    {
        foreach (var replacement in _replacements)
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
