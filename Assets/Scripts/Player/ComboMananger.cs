using UnityEngine;

public class ComboMananger : MonoBehaviour
{
    public static ComboMananger Instance { get; private set; }

    int comboCount = 0;
    float durationSinceLastAppleEatAudio = 0f;
    const float DURATION_RESET_COMBO = 2f;

    [SerializeField]
    private ScoreText scoreText;
    [SerializeField]
    private GameObject comboIndicatorPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Reset()
    {
        comboCount = 0;
        durationSinceLastAppleEatAudio = 0f;
        scoreText.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        // Reset the pitch after a certain duration has passed since the last time the snake ate an apple
        durationSinceLastAppleEatAudio += Time.deltaTime;
        if (durationSinceLastAppleEatAudio >= DURATION_RESET_COMBO)
        {
            comboCount = 0;
        }
    }

    public void IncrementCombo(Vector2 _posApple)
    {
        comboCount++;
        durationSinceLastAppleEatAudio = 0f;

        GameObject _comboIndicatorTxt = Instantiate(comboIndicatorPrefab, _posApple, Quaternion.identity);
        ComboIndicator _comboIndicator1 = _comboIndicatorTxt.GetComponent<ComboIndicator>();
        if (_comboIndicator1 != null)
        {
            _comboIndicator1.Setup(comboCount, 0.75f);
        }
        SetupShakeScore();
    }

    void SetupShakeScore()
    {
        if (comboCount <= 1)
        {
            scoreText.ShakeText(1f, 0.2f);
        }
        else
        {
            int _comboPoints = Mathf.Min(comboCount, 5);
            scoreText.ShakeText(DURATION_RESET_COMBO, 0.5f * _comboPoints, true);
        }
    }

    public int GetComboCount()
    {
        return comboCount;
    }
}
