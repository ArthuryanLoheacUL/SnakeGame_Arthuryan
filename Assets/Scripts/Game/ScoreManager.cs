using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score = 0;
    public TMP_Text scoreText;

    public AudioClip scoreSoundEffect;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetScore();
    }

    public void AddScore(int _points)
    {
        SetScore(score + _points);
        SoundEffectManager.instance.PlayAudioSourceRandomPitched(scoreSoundEffect, 0.5f, 0.1f);
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        SetScore(0);
    }

    void SetScore(int _score)
    {
        score = _score;
        RefreshScoreText();
    }

    void RefreshScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}
