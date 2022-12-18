using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

internal class GenerateCardEffects : MonoBehaviour
{
    private const string FILE_PATH = "Assets/Resources/CardEffects";
#if UNITY_EDITOR

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void Generate()
    {
        var cardEffects = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
        .Where(type => type.IsSubclassOf(typeof(CardEffect)))
        .Where(type => !AssetDatabase.FindAssets($"t:{type.Name}", new string[] { FILE_PATH }).Any())
        .ToList();

        foreach (var cardEffect in cardEffects)
        {
            var asset = ScriptableObject.CreateInstance(cardEffect);
            AssetDatabase.CreateAsset(asset, FILE_PATH + $"/{cardEffect.Name}.asset");
        }
    }
}
#endif

