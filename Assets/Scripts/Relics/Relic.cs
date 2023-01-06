using UnityEngine;

public abstract class Relic : ScriptableObject
{
    [SerializeField]
    private string _displayName;
    [SerializeField]
    private string _description;
}