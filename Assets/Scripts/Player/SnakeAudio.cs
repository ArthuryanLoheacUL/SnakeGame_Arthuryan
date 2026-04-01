using UnityEngine;

public class SnakeAudio : MonoBehaviour
{
    public SoundEffectManager.SnakeAudioClip appleEatAudio;
    public SoundEffectManager.SnakeAudioClip appleEatComboedAudio;
    public SoundEffectManager.SnakeAudioClip hitWallAudio;
    public SoundEffectManager.SnakeAudioClip snakeHissAudio;

    const float PITCH_INCREMENT_EAT_AUDIO = 0.05f;
    const float MIN_PITCH_EAT_AUDIO = 0.8f;

    // Play the apple eat sound effect with a pitch that increases each time the snake eats an apple
    public void PlayAppleEatSound()
    {
        float _pitch = MIN_PITCH_EAT_AUDIO;
        if (ComboMananger.Instance != null)
        {
            int _comboCount = Mathf.Min(ComboMananger.Instance.GetComboCount(), 5);
            _pitch += (_comboCount - 1) * PITCH_INCREMENT_EAT_AUDIO;
        }

        SoundEffectManager.instance.PlayAudioSourceSetPitch(appleEatAudio, _pitch);
        if (ComboMananger.Instance != null && ComboMananger.Instance.GetComboCount() >= 5)
        {
            SoundEffectManager.instance.PlayAudioSourceSetPitch(appleEatComboedAudio, _pitch);
        }
    }

    // Play the hit wall sounds effect
    public void PlayHitWallSound()
    {
        SoundEffectManager.instance.PlayAudioSourceRandomPitched(hitWallAudio);
        SoundEffectManager.instance.PlayAudioSourceRandomPitched(snakeHissAudio);
    }
}
