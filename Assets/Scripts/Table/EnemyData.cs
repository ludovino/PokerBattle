using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyData : EntityData
{
    public string displayName;
    public int chipsPerLevel;

    public void LevelUp()
    {
        ChangeChips(chipsPerLevel);
        var card = _factory.GetRandomCards(1).First();
        AddCard(card);
    }
}

