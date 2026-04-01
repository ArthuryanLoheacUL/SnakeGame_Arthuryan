using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text multiplierText;
    private Vector3 originalPosition;
    [SerializeField]
    private Slider sliderTimeCombo;

    float durationShake = 0f;
    float durationMax = 0f;
    float intensityShake = 0f;
    bool isCombo = false;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        originalPosition = scoreText.transform.localPosition;
        multiplierText.gameObject.SetActive(false);
        sliderTimeCombo.gameObject.SetActive(false);
    }

    void Update()
    {
        if (durationShake > 0)
        {
            durationShake -= Time.deltaTime;
            scoreText.transform.localPosition = originalPosition + Random.insideUnitSphere * intensityShake;
            sliderTimeCombo.gameObject.SetActive(isCombo);
            sliderTimeCombo.value = durationShake / durationMax;
            if (durationShake <= 0)
            {
                durationShake = 0;
                scoreText.transform.localPosition = originalPosition;
                sliderTimeCombo.gameObject.SetActive(false);
            }
        }
        if (ComboMananger.Instance != null)
        {
            multiplierText.gameObject.SetActive(ComboMananger.Instance.GetComboCount() > 1);
            multiplierText.text = "x" + ComboMananger.Instance.GetComboCount();
            int _maxCombo = Mathf.Min(ComboMananger.Instance.GetComboCount(), 5) - 1;
            multiplierText.fontSize = 18 + _maxCombo * 2;
        }
    }

    public void Reset()
    {
        scoreText.text = "0";
        multiplierText.gameObject.SetActive(false);
        intensityShake = 0f;
        durationShake = 0f;
    }

    public void ShakeText(float _intensity, float _duration, bool _isCombo = false)
    {
        intensityShake = _intensity;
        if (durationShake > 0 && durationShake < _duration || durationShake <= 0)
        {
            durationShake = _duration;
            durationMax = _duration;
        }
        isCombo = _isCombo;
    }
}
