using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float time;
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(this.gameObject, time);
    }
}
