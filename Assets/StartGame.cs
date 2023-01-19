using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void Go()
    {
        GameController.Instance.BeginGame();
    }
}
