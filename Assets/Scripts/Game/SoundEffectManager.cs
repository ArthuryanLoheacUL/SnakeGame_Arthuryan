using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;
    private AudioSource audioSource;

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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on SoundEffectManager. Please add an AudioSource component.");
        }
    }

    public void PlayAudioSourcePitched(AudioClip _clip, float _volume = 1f, float _deltaPitch = 0.2f)
    {
        if (audioSource != null && _clip != null)
        {
            audioSource.pitch = Random.Range(1f - _deltaPitch, 1f + _deltaPitch);
            audioSource.PlayOneShot(_clip, _volume);
        }
    }
}
