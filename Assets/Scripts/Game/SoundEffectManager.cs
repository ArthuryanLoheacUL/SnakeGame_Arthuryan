using System.Collections;
using UnityEngine;

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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("AudioSource component not found on SoundEffectManager.");
    }

    public void PlayAudioSourceRandomPitched(SnakeAudioClip _snakeAudioClip, float _deltaPitch = 0.2f)
    {
        PlayAudioSourceRandomPitched(_snakeAudioClip.clip, _snakeAudioClip.volume, _deltaPitch);
    }

    public void PlayAudioSourceRandomPitched(AudioClip _clip, float _volume = 1f, float _deltaPitch = 0.2f)
    {
        if (audioSource == null || _clip == null) return;
        float _pitch = Random.Range(1f - _deltaPitch, 1f + _deltaPitch);
        audioSource.pitch = _pitch;
        audioSource.PlayOneShot(_clip, _volume);
    }

    public void PlayAudioSourceSetPitch(SnakeAudioClip _snakeAudioClip, float _pitch = 1f)
    {
        PlayAudioSourceSetPitch(_snakeAudioClip.clip, _snakeAudioClip.volume, _pitch);
    }

    public void PlayAudioSourceSetPitch(AudioClip _clip, float _volume = 1f, float _pitch = 1f)
    {
        if (audioSource == null || _clip == null) return;
        audioSource.pitch = _pitch;
        audioSource.PlayOneShot(_clip, _volume);
    }
}