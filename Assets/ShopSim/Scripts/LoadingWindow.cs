using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingWindow : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialWindow;
    [SerializeField] private GameObject _loadingWindow;
    [SerializeField] private GameObject _loadingText;
    [SerializeField] private string nextSceneName;

    private void Start()
    {
        int isTutorial = PlayerPrefs.GetInt("tutorial", 0);

        if (isTutorial == 1)
        {
            Invoke("LoadNextScene", 3f);
        }
        else
        {
            Invoke("ShowTutorialWindow", 3f);
        }
    }

    private void ShowTutorialWindow()
    {
        _loadingWindow.SetActive(false);
        _loadingText.SetActive(false);
        _tutorialWindow.SetActive(true);
    }
    
    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void LoadSceneMain()
    {
        PlayerPrefs.SetInt("tutorial", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main");
    }
}
