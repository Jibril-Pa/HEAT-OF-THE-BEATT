using UnityEngine;

public class playAudioOnTriggerEnter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip clip;
    private AudioSource source;
    public string Tag;

    public bool useVelocity = true;
    public float minVelocity = 0;
    public float maxVelocity = 2;
    void Start()
    {
     source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag))
        {
            VelocityEstimator velocityEstimator = other.GetComponent<VelocityEstimator>();
            if (velocityEstimator && useVelocity)
            {
                float v = velocityEstimator.GetVelocityEstimate().magnitude;
                float volume = Mathf.InverseLerp(minVelocity, maxVelocity, v);

                source.PlayOneShot(clip, volume);
            }


            source.PlayOneShot(clip);
        }
    }
    
}

