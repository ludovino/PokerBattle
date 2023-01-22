using System;
using System.Reflection;
using UnityEngine;

public abstract partial class Unlockable : ScriptableObject
{
    [SerializeField]
    private int _score;
    public int Score => _score;
    protected abstract UnlockDisplay DoUnlock();
    public void Unlock()
    {
        // unlock pop-up;
    }
}
