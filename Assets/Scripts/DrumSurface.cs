using UnityEngine;

public class DrumSurface : MonoBehaviour
{
    public AudioClip drumSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = drumSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
    }

    public void PlayDrumSound(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.Play();
    }
}
