using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Scenario")]
public class Scenario : ScriptableObject
{
    [SerializeField]
    private string _title;
    public string Title => _title;
    [SerializeField, TextArea(3, 10)]
    private string _description;
    public string Description => _description;

    [SerializeField]
    private List<Outcome> _outcomes;

    public List<Outcome> Outcomes => _outcomes;
}