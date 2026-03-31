using UnityEngine;

public class SnakeAudio : MonoBehaviour
{
    public AudioClip appleEatAudio;

    public void PlayAppleEatSound()
    {
        SoundEffectManager.instance.PlayAudioSourcePitched(appleEatAudio, 1.5f);
    }
}
