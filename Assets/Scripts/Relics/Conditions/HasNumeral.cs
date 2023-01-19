using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "Relic/Condition/Numeral")]
public class HasNumeral : RelicCondition
{
    [SerializeField]
    private string _numeral;
    [SerializeField]
    private int _count;
    public override bool Met()
    {
        return PlayerData.Instance.Cards.Count(c => c.numeral == _numeral) >= _count;
    }
}

