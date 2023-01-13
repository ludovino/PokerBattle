using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Relic/Pool")]
public class RelicPool : ScriptableObject
{
    [SerializeField]
    private List<Relic> _relics;

}
