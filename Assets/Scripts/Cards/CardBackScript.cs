using UnityEngine;
public class CardBackScript : MonoBehaviour
{
    [SerializeField]
    private GameObject back;
    public bool faceUp => Vector3.Project(transform.forward, Vector3.forward).z > 0;
    // Update is called once per frame
    void Update()
    {
        back.SetActive(!faceUp);
    }
}
