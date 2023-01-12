using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Entities/PlayerData")]
public class PlayerData : EntityData
{
    private static PlayerData _instance;
    public static PlayerData Instance => _instance != null ? _instance : SetInstance();

    void OnEnable()
    {
        Init();
        SetInstance();
    }

    private static PlayerData SetInstance()
    {
        return _instance = Resources.Load<PlayerData>("Player");
    }
}

