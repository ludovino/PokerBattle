using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    private Coroutine _executing;

    private void Awake()
    {
        _queue = new Queue<IEnumerator>();
    }

    private void Update() 
    {
        CheckQueue();
    }

    private IEnumerator CR_Execute(IEnumerator coroutine)
    {
        _executing = StartCoroutine(coroutine);
        yield return _executing;
        _executing = null;
    }

    private void CheckQueue()
    {
        if(_executing != null)
        {
            return;
        }
        if(_queue.TryDequeue(out var next))
        {
            StartCoroutine(CR_Execute(next));
        }
    }

    public static void Defer(IEnumerator coroutine)
    {
        Instance._queue.Enqueue(coroutine);
        Instance.CheckQueue();
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
            StopCoroutine(_executing);
        }
    }

    private void OnDestroy()
    {
        Clear(true);
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

