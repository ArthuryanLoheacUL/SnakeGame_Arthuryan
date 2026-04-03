using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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
    private AudioSource[] audioSources;

    List<string> toPlayThisFrame = new List<string>();
    float timerClearList = 0f;

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
        audioSources = GetComponents<AudioSource>();
        if (audioSources.Length == 0)
            Debug.LogError("AudioSource component not found on SoundEffectManager.");
    }

    public void PlayAudioSourceRandomPitched(SnakeAudioClip _snakeAudioClip, float _deltaPitch = 0.2f, int _source = 0)
    {
        PlayAudioSourceRandomPitched(_snakeAudioClip.clip, _snakeAudioClip.volume, _deltaPitch, _source);
    }

    public void PlayAudioSourceRandomPitched(AudioClip _clip, float _volume = 1f, float _deltaPitch = 0.2f, int _source = 0)
    {
        AudioSource _audioSource = GetAudioSource(_source);
        if (_audioSource == null || _clip == null) return;
        if (toPlayThisFrame.Contains(_clip.name)) return;
        float _pitch = Random.Range(1f - _deltaPitch, 1f + _deltaPitch);
        _audioSource.pitch = _pitch;
        _audioSource.PlayOneShot(_clip, _volume);
        toPlayThisFrame.Add(_clip.name);
    }

    public void PlayAudioSourceSetPitch(SnakeAudioClip _snakeAudioClip, float _pitch = 1f, int _source = 0)
    {
        PlayAudioSourceSetPitch(_snakeAudioClip.clip, _snakeAudioClip.volume, _pitch);
    }

    public void PlayAudioSourceSetPitch(AudioClip _clip, float _volume = 1f, float _pitch = 1f, int _source = 0)
    {
        AudioSource _audioSource = GetAudioSource(_source);
        if (_audioSource == null || _clip == null) return;
        if (toPlayThisFrame.Contains(_clip.name)) return;
        _audioSource.pitch = _pitch;
        _audioSource.PlayOneShot(_clip, _volume);
        toPlayThisFrame.Add(_clip.name);
    }

    AudioSource GetAudioSource(int _source)
    {
        if (_source < 0) return null;
        if (_source >= audioSources.Length)
        {
            for (int _i = audioSources.Length; _i <= _source; _i++)
            {
                AudioSource _newAudioSource = gameObject.AddComponent<AudioSource>();
                _newAudioSource.playOnAwake = false;
                audioSources = GetComponents<AudioSource>();
            }
        }
        return audioSources[_source];
    }

    void Update()
    {
        timerClearList += Time.deltaTime;
        if (timerClearList >= 0.05f)
        {
            toPlayThisFrame.Clear();
            timerClearList = 0f;
        }
    }
}