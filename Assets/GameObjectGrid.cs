using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectGrid : MonoBehaviour
{
    [SerializeField]
    private float scaleFactor;
    [SerializeField]
    private Transform _objectParent; 
    private List<Transform> _transforms;
    private List<GameObject> _gameObjects;
    private void Awake()
    {
        InitCollections();
    }

    private void InitCollections()
    {
        _gameObjects = _gameObjects ?? new List<GameObject>();
        _transforms = _transforms ?? new List<Transform>();
        _objectParent.transform.localScale = Vector3.one * scaleFactor;
    }

    public void Add(GameObject gameObject)
    {
        var slot = new GameObject("CardSlot", typeof(RectTransform));
        slot.transform.SetParent(transform, false);
        gameObject.transform.localPosition = slot.transform.position - Vector3.forward;
        gameObject.transform.parent = _objectParent;
        gameObject.transform.localScale = Vector3.one;
        _transforms.Add(slot.transform);
        _gameObjects.Add(gameObject);
    }
    private void Update()
    {
        for(int i = 0; i < _gameObjects.Count; i++)
        {
            _gameObjects[i].transform.position = _transforms[i].position - Vector3.forward;
        }
    }
    public void Clear() 
    {
        InitCollections();
        _gameObjects.Clear();
        _transforms.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
            Destroy(_objectParent.GetChild(i).gameObject);
        }
    }
}
