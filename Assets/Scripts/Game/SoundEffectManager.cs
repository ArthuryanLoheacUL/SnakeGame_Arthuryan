using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public struct SnakeAudioClip
{
    public AudioClip clip;
    [Range(0f, 2f)]
    public float volume;
}

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;
    private AudioSource audioSource;

    // Awake is called when the script instance is being loaded
    void Awake()
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

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on SoundEffectManager. Please add an AudioSource component.");
        }
    }

    // Play the specified audio clip with random pitch variation
    public void PlayAudioSourceRandomPitched(SnakeAudioClip _snakeAudioClip, float _deltaPitch = 0.2f)
    {
        PlayAudioSourceRandomPitched(_snakeAudioClip.clip, _snakeAudioClip.volume, _deltaPitch);
    }

    // Play the specified audio clip with random pitch variation and given volume
    public void PlayAudioSourceRandomPitched(AudioClip _clip, float _volume = 1f, float _deltaPitch = 0.2f)
    {
        if (audioSource != null && _clip != null)
        {
            audioSource.pitch = Random.Range(1f - _deltaPitch, 1f + _deltaPitch);
            audioSource.PlayOneShot(_clip, _volume);
        }
    }

    // Play the specified audio clip with the given pitch
    public void PlayAudioSourceSetPitch(SnakeAudioClip _snakeAudioClip, float _pitch = 1f)
    {
        PlayAudioSourceSetPitch(_snakeAudioClip.clip, _snakeAudioClip.volume, _pitch);
    }

    // Play the specified audio clip with the given pitch and volume
    public void PlayAudioSourceSetPitch(AudioClip _clip, float _volume = 1f, float _pitch = 1f)
    {
        if (audioSource != null && _clip != null)
        {
            audioSource.pitch = _pitch;
            audioSource.PlayOneShot(_clip, _volume);
        }
    }
}
