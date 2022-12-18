using UnityEngine;
public class CardBackScript : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer back;
    public bool faceUp => Vector3.Project(transform.forward, Vector3.forward).z > 0;
    // Update is called once per frame
    void Update()
    {
        if (faceUp) back.sortingOrder = -1;
        else back.sortingOrder = 2;
    }
}
