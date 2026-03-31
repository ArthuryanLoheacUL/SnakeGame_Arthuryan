using UnityEngine;

public class SnakeAudio : MonoBehaviour
{
    public AudioClip appleEatAudio;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAppleEatSound()
    {
        PlayAudioSourcePitched(appleEatAudio);
    }

    void PlayAudioSourcePitched(AudioClip _clip, float _deltaPitch = 0.2f)
    {
        if (audioSource != null && _clip != null)
        {
            audioSource.pitch = Random.Range(1f - _deltaPitch, 1f + _deltaPitch);
            audioSource.PlayOneShot(_clip);
        }
    }
}
