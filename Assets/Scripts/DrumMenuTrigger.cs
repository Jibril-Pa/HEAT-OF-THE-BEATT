using UnityEngine;
using UnityEngine.SceneManagement;

public class DrumMenuTrigger : MonoBehaviour
{
    public enum DrumFunction { OpenSongPanel, StartGame }
    public DrumFunction drumType;

    public GameObject canvasToShow;
    public GameObject canvasToHide;
    public string sceneToLoad;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Drumstick")) return;

        switch (drumType)
        {
            case DrumFunction.OpenSongPanel:
                if (canvasToHide) canvasToHide.SetActive(false);
                if (canvasToShow) canvasToShow.SetActive(true);
                break;
            case DrumFunction.StartGame:
                if (!string.IsNullOrEmpty(sceneToLoad))
                    SceneManager.LoadScene(sceneToLoad);
                break;
        }
    }
}
