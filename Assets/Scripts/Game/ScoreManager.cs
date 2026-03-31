using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score = 0;
    [SerializeField]
    public TMP_Text scoreText;

    [SerializeField]
    private TMP_Text targetScoreText;
    private int targetScore = 100;
    private bool targetScoreReached = false;

    [SerializeField]
    private TMP_Text highScoreText;
    private int highScore = 0;

    public AudioClip scoreSoundEffect;
    public AudioClip targetReached;

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
        if (score >= targetScore && !targetScoreReached)
        {
            SoundEffectManager.instance.PlayAudioSourceRandomPitched(targetReached, 0.5f, 0);
            targetScoreReached = true;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        SetScore(0);
        targetScoreReached = false;
    }

    public void SetTargetScore(int _targetScore)
    {
        targetScore = _targetScore;
        RefreshTargetScoreText();
    }

    public void SetHighScore(int _highScore)
    {
        highScore = _highScore;
        RefreshHighScoreText();
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

    void RefreshHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = highScore.ToString();
        }
    }

    void RefreshTargetScoreText()
    {
        if (targetScoreText != null)
        {
            targetScoreText.text = targetScore.ToString();
        }
    }
}
