using UnityEngine;

public class ComboMananger : MonoBehaviour
{
    int comboCount = 0;
    float durationSinceLastAppleEatAudio = 0f;
    const float DURATION_RESET_COMBO = 2f;

    public GameObject comboIndicatorPrefab;

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
    }

    public int GetComboCount()
    {
        return comboCount;
    }
}
