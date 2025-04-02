using UnityEngine;
using System.Collections.Generic;

public class DrumStick : MonoBehaviour
{
    public float minVelocity = 0.5f;
    public float maxVolumeVelocity = 3f;

    // List of valid drum tags
    private readonly HashSet<string> validDrumTags = new HashSet<string>
    {
        "snare", "tom1", "tom2", "hi_hat1", "hi_hat2", "cymballs", "tom3"
    };

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        if (validDrumTags.Contains(tag))
        {
            float velocity = collision.relativeVelocity.magnitude;
            if (velocity > minVelocity)
            {
                DrumSurface drum = collision.gameObject.GetComponent<DrumSurface>();
                if (drum != null)
                {
                    float volume = Mathf.Clamp01(velocity / maxVolumeVelocity);
                    drum.PlayDrumSound(volume);
                }

                // Optional: add haptic feedback here
            }
        }
    }
}
