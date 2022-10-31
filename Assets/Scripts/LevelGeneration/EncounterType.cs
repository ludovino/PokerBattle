using System;
using UnityEngine;
public abstract class EncounterType : ScriptableObject
{
    public Sprite mapSprite;
    public abstract Encounter GetEncounter();
}
