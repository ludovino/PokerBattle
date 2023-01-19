using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Relic/Condition/Suit")]
public class HasSuit : RelicCondition
{
    [SerializeField]
    private Suit _suit;
    [SerializeField]
    private SuitList _suitList;
    public override bool Met()
    {
        return _suitList.Contains(_suit);
    }
}

