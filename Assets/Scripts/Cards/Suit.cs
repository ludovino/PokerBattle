using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "Suit")]
public class Suit : ScriptableObject
{
    public string longName;
    public string shortName;
    public string display;
    public Sprite sprite;
    [SerializeField]
    private CardColor _color;
    public CardColor Color => _color;
    [SerializeField]
    private FaceSprite[] faceSprites;
    public IReadOnlyDictionary<Face, Sprite> Faces { get; private set; }
    private void Awake()
    {
        faceSprites = faceSprites ?? Resources.LoadAll<Face>("Faces").Select(f => new FaceSprite() { face = f }).ToArray();
    }

    private void OnEnable()
    {
        Faces = faceSprites.ToDictionary(f => f.face, f => f.sprite);
    }
}
[Serializable]
public class FaceSprite
{
    public Face face;
    public Sprite sprite;
}
