using UnityEngine;

public class SnakeAudio : MonoBehaviour
{
    public SoundEffectManager.SnakeAudioClip appleEatAudio;
    public SoundEffectManager.SnakeAudioClip hitWallAudio;
    public SoundEffectManager.SnakeAudioClip snakeHissAudio;

    float pitchEatAudio = 0.8f;
    float durationSinceLastAppleEatAudio = 0f;
    const float MAX_PITCH_EAT_AUDIO = 1.5f;
    const float PITCH_INCREMENT_EAT_AUDIO = 0.05f;
    const float DURATION_RESET_PITCH_EAT_AUDIO = 2.5f;
    const float MIN_PITCH_EAT_AUDIO = 0.8f;

    public void PlayAppleEatSound()
    {
        SoundEffectManager.instance.PlayAudioSourceSetPitch(appleEatAudio, pitchEatAudio);
        pitchEatAudio += PITCH_INCREMENT_EAT_AUDIO;
        if (pitchEatAudio > MAX_PITCH_EAT_AUDIO)
        {
            pitchEatAudio = MAX_PITCH_EAT_AUDIO;
        }
        durationSinceLastAppleEatAudio = 0f;
    }

    void Update()
    {
        if (pitchEatAudio > MIN_PITCH_EAT_AUDIO)
        {
            durationSinceLastAppleEatAudio += Time.deltaTime;
            if (durationSinceLastAppleEatAudio >= DURATION_RESET_PITCH_EAT_AUDIO)
            {
                pitchEatAudio = MIN_PITCH_EAT_AUDIO;
                durationSinceLastAppleEatAudio = 0f;
            }
        }
    }

    public void PlayHitWallSound()
    {
        SoundEffectManager.instance.PlayAudioSourceRandomPitched(hitWallAudio);
        SoundEffectManager.instance.PlayAudioSourceRandomPitched(snakeHissAudio);
    }
}
