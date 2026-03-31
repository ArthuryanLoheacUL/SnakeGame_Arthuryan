using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score = 0;
    public TMP_Text scoreText;

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
