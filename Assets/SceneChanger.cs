using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }
    public Image image;
    public float fadeTime;
    public void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    public void ChangeScene(string sceneName, System.Action onComplete = null)
    {
        image.DOColor(new Color(0, 0, 0, 1), fadeTime).OnComplete(() => LoadScene(sceneName, onComplete));
    }

    private void LoadScene(string sceneName, System.Action onComplete = null)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName);
        if(onComplete != null) operation.completed += (op) => onComplete();
        operation.completed += (op) => image.DOColor(new Color(0, 0, 0, 0), fadeTime).SetDelay(0.2f);
    }
}
