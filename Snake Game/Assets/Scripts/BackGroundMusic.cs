using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    public AudioClip musicTrack;
    [Range(0, 1)] public float volume = 0.5f;

    void Start()
    {
        // Set up audio source
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicTrack;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();

        // Make this object persist across scenes
        DontDestroyOnLoad(gameObject);
    }
}