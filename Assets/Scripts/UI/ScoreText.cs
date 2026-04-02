using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private TMP_Text scoreText;
    private Vector3 originalPosition;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private Slider sliderTimeCombo;

    private float durationShake = 0f;
    private float durationMax = 0f;
    private float intensityShake = 0f;
    private bool isCombo = false;

    [SerializeField] private Color colorSimpleCombo = Color.white;
    [SerializeField] private Color colorChargedCombo = Color.yellow;

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
            if (sliderTimeCombo.fillRect.GetComponent<Image>())
                sliderTimeCombo.fillRect.GetComponent<Image>().color = (ComboMananger.Instance.GetComboCount() >= 5 ? colorChargedCombo : colorSimpleCombo);
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
        sliderTimeCombo.gameObject.SetActive(false);
    }

    public void ShakeText(float _intensity, float _duration, bool _isCombo = false)
    {
        // Only update the shake if the new duration is longer than the current one, or if there is no shake currently active
        if (durationShake > 0 && durationShake < _duration || durationShake <= 0)
        {
            intensityShake = _intensity;
            durationShake = _duration;
            durationMax = _duration;
            isCombo = _isCombo;
        }
    }
}
