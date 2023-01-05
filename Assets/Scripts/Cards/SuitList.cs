
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "Card/SuitList")]
public class SuitList : ScriptableObject
{
    [SerializeField]
    private List<Suit> _suits;
    public IReadOnlyList<Suit> Suits => _suits;

    public void SetSuits(List<Suit> suits, int suitCount = 5)
    {
        _suits.Clear();
        _suits.AddRange(suits);
        if (_suits.Count < suitCount)
        {
            _suits.AddRange(Enumerable.Repeat<Suit>(null, suitCount - _suits.Count));
        }
    }
}

