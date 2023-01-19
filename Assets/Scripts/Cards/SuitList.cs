
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "Card/SuitList")]
public class SuitList : ScriptableObject, IReadOnlyList<Suit>
{
    [SerializeField]
    private List<Suit> _suits;

    public Suit this[int index] => ((IReadOnlyList<Suit>)_suits)[index];

    public int Count => ((IReadOnlyCollection<Suit>)_suits).Count;

    public IEnumerator<Suit> GetEnumerator()
    {
        return ((IEnumerable<Suit>)_suits).GetEnumerator();
    }

    public void SetSuits(List<Suit> suits, int suitCount = 5)
    {
        _suits.Clear();
        _suits.AddRange(suits);
        if (_suits.Count < suitCount)
        {
            _suits.AddRange(Enumerable.Repeat<Suit>(null, suitCount - _suits.Count));
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_suits).GetEnumerator();
    }
}

