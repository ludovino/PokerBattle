using System;
using UnityEngine;
public abstract class EncounterType : ScriptableObject
{
    public Sprite mapSprite;

    public string DisplayName;

    public abstract Encounter GetEncounter();
}
