using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CardChanger : MonoBehaviour
{
    public Sprite Sprite;
    public CardScript Target;
    public Color ChangeColor = Color.black;
    public UnityEvent onChanged;
    public float ChangeTime;
    private void Awake()
    {
        onChanged ??= new UnityEvent();
    }
    public void Change()
    {
        Target.SetSprite(Sprite, ChangeColor, ChangeTime);
        onChanged.Invoke();
    }
}
