using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class SongManager : MonoBehaviour
{
    public Button playButton;

    void Start()
    {
        playButton.onClick.AddListener(LoadDrumsetScene);
    }

    // This method is called when the Play button is pressed
    void LoadDrumsetScene()
    {
        Debug.Log("Loading Drumset scene...");
        SceneManager.LoadScene("Drumset"); // Load the Drumset scene
    }
}
