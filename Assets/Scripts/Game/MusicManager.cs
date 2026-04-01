using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play the specified music clip with the given volume
    public void PlayMusic(AudioClip _musicClip, float _volume = 1f)
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
        audioSource.clip = _musicClip;
        audioSource.volume = _volume;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Stop the currently playing music if there is one
    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
