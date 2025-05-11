using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject songPanel;
    public GameObject mainMenuPanel;

    public void OnPlayClicked()
    {
        mainMenuPanel.SetActive(false);
        songPanel.SetActive(true);
    }
    public void OnExitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnSongSelected(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
