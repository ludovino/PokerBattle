using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Cards
{
    [CreateAssetMenu(menuName = "Card/SpriteCollection")]
    public class CardSpriteCollection : ScriptableObject
    {
        [SerializeField]
        private string _folder;
        [SerializeField]
        private List<Sprite> _sprites;
        private Dictionary<string, Sprite> _spriteDict;
#if UNITY_EDITOR
        [Button]
        private void Populate()
        {
            var assetIds = AssetDatabase.FindAssets("t:Sprite", new string[] { _folder });
            _sprites = assetIds
                .Select(id => AssetDatabase.GUIDToAssetPath(id))
                .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)))
                .Cast<Sprite>().ToList();
        }
#endif
        private void OnEnable()
        {
            _spriteDict = _sprites.ToDictionary(s => s.name, s => s);
        }

        public Sprite Get(ICard card)
        {
            var numeral = card.highCardRank.ToString("D2");
            if (card.face != null) numeral = card.face.numeral;
            var suitName = "blank";
            if (card.suit != null) suitName = card.suit.name;
            return _spriteDict[$"{numeral}Of{suitName}"];
        }
    }
}
