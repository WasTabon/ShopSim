using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingWindow : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private void Start()
    {
        Invoke("LoadNextScene", 3f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
