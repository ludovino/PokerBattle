using UnityEngine;
public class CardBackScript : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer back;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Project(this.transform.forward, Vector3.forward).z > 0) back.sortingOrder = -1;
        else back.sortingOrder = 2;
    }
}
