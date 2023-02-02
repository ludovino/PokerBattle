using System;
using System.Reflection;
using UnityEngine;

public abstract partial class Unlockable : ScriptableObject
{
    [SerializeField]
    private int _score;
    public int Score => _score;
    public abstract UnlockDisplay GetDisplay();
}
