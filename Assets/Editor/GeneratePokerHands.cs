using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class GeneratePokerHands : MonoBehaviour
{
    private const string FILE_PATH = "Assets/Resources/Hands";
    #if UNITY_EDITOR
    
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void Generate()
    {
        var handTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
        .Where(type => type.IsSubclassOf(typeof(PokerHand)))
        .Where(type => !AssetDatabase.FindAssets($"t:{type.Name}",new string[]{FILE_PATH}).Any())
        .ToList();
        
        foreach(var handType in handTypes)
        {
            var asset = ScriptableObject.CreateInstance(handType);
            AssetDatabase.CreateAsset(asset, FILE_PATH + $"/{handType.Name}.asset");
        }
    }
    #endif
}
