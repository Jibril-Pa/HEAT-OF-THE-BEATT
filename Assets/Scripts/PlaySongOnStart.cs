using UnityEngine;

public class PlaySongOnStart : MonoBehaviour
{
    public AudioClip song;         // Assign in the Inspector
    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.clip = song;
        audioSrc.playOnAwake = false;  // Prevent auto-play until we call Play()
        audioSrc.loop = false;         // Set to true if you want it to loop
        audioSrc.Play();
    }
}
