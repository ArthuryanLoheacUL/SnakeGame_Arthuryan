using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private AudioClip scoreSoundEffect;
    private int score = 0;

    [SerializeField] private TMP_Text targetScoreText;
    [SerializeField] private AudioClip targetReached;
    private int targetScore = 100;
    private bool targetScoreReached = false;

    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private AudioClip highScoreSoundEffect;
    private int highScore = 0;
    private bool highScoreReached = false;


    // Awake is called when the script instance is being loaded
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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("highscore" + SceneManager.GetActiveScene().buildIndex.ToString()))
        {
            SetHighScore(PlayerPrefs.GetInt("highscore" + SceneManager.GetActiveScene().buildIndex.ToString()));
        } else
        {
            SetHighScore(0);
            highScoreReached = true;
        }
        ResetScore();
    }

    // Add the specified points to the current score
    public void AddScore(int _points)
    {
        SetScore(score + _points);
        SoundEffectManager.instance.PlayAudioSourceRandomPitched(scoreSoundEffect, 0.5f, 0.1f);
        if (score >= targetScore && !targetScoreReached)
        {
            SoundEffectManager.instance.PlayAudioSourceRandomPitched(targetReached, 0.5f, 0);
            targetScoreReached = true;
        }
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highscore" + SceneManager.GetActiveScene().buildIndex.ToString(), score);
            SetHighScore(score);
            if (!highScoreReached)
            {
                SoundEffectManager.instance.PlayAudioSourceRandomPitched(highScoreSoundEffect, 0.75f, 0);
                highScoreReached = true;
            }
        }
    }

    // Get the current score
    public int GetScore()
    {
        return score;
    }

    // Reset the score to zero and update the target and high score reached flags
    public void ResetScore()
    {
        SetScore(0);
        targetScoreReached = false;
        highScoreReached = highScore <= 0;
    }

    // Set the target score and update the target score text
    public void SetTargetScore(int _targetScore)
    {
        targetScore = _targetScore;
        RefreshTargetScoreText();
    }

    // Set the high score and update the high score text
    public void SetHighScore(int _highScore)
    {
        highScore = _highScore;
        RefreshHighScoreText();
    }

    // Set the current score and update the score text
    void SetScore(int _score)
    {
        score = _score;
        RefreshScoreText();
    }

    // Refresh the score text to display the current score
    void RefreshScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    // Refresh the high score text to display the current high score
    void RefreshHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = highScore.ToString();
        }
    }

    // Refresh the target score text to display the current target score
    void RefreshTargetScoreText()
    {
        if (targetScoreText != null)
        {
            targetScoreText.text = targetScore.ToString();
        }
    }
}
