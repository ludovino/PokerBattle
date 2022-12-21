using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoroutineQueue : MonoBehaviour
{
    private static CoroutineQueue _instance;
    public static CoroutineQueue Instance 
    { 
        get 
        {
            if (_instance == null)
            {
                GameObject gObj = new GameObject();
                gObj.name = "CoroutineQueue";
                _instance = gObj.AddComponent<CoroutineQueue>();
            }
            return _instance;
        } 
        private set => _instance = value;  
    }
    private Queue<IEnumerator> _queue;
    private Coroutine _crHandler;
    private void Awake()
    {
        _queue = new Queue<IEnumerator>();
    }
    private void Start()
    {
        _crHandler = StartCoroutine(CR_Handler());
    }
    private IEnumerator CR_Handler()
    {
        while (true)
        {
            if(_queue.TryDequeue(out var next))
            {
                yield return StartCoroutine(next);
            }
            yield return null;
        }
    }

    public static void Defer(IEnumerator coroutine)
    {
        Instance._queue.Enqueue(coroutine);
    }

    public static void ClearQueue(bool stopCurrent)
    {
        Instance.Clear(stopCurrent);
    }
    private void Clear(bool stopCurrent)
    {
        _queue.Clear();
        if (stopCurrent)
        {
            StopCoroutine(_crHandler);
            _crHandler = StartCoroutine(CR_Handler());
        }
    }

    private void OnDestroy()
    {
        _queue.Clear();
        StopCoroutine(_crHandler);
    }

    private class Parallel
    {
        private CoroutineQueue q;
        public List<IEnumerator> coroutines;
        IEnumerator Execute()
        {
            var crs = coroutines.Select(cr => q.StartCoroutine(cr));
            foreach(var cr in crs)
            {
                yield return cr;
            }
        }
    }
}

