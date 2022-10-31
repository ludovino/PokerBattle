using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectGrid : MonoBehaviour
{
    [SerializeField]
    private float scaleFactor;
    private List<GameObject> _gameObjects;
    private void Awake()
    {
        InitCollections();
    }

    private void InitCollections()
    {
        _gameObjects = _gameObjects ?? new List<GameObject>();
    }
    public void Add(GameObject gameObject)
    {
        var slot = new GameObject("CardSlot", typeof(RectTransform));
        slot.transform.SetParent(transform, false);
        slot.transform.localScale = Vector3.one * scaleFactor;
        gameObject.transform.SetParent(slot.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        _gameObjects.Add(gameObject);
    }

    public GameObject Take(GameObject gameObject)
    {
        _gameObjects.Remove(gameObject);
        var slot = gameObject.transform.parent;
        gameObject.transform.SetParent(null, true);
        Destroy(slot.gameObject);
        return gameObject;
    }

    public void Clear() 
    {
        InitCollections();
        _gameObjects.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
