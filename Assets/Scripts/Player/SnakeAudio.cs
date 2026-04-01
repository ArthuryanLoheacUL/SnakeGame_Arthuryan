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

    // Play the apple eat sound effect with a pitch that increases each time the snake eats an apple
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

    // Update is called once per frame
    void Update()
    {
        // Reset the pitch after a certain duration has passed since the last time the snake ate an apple
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

    // Play the hit wall sounds effect
    public void PlayHitWallSound()
    {
        SoundEffectManager.instance.PlayAudioSourceRandomPitched(hitWallAudio);
        SoundEffectManager.instance.PlayAudioSourceRandomPitched(snakeHissAudio);
    }
}
