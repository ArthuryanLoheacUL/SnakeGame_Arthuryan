using UnityEngine;
using TMPro;

public class ComboIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI comboText;
    private Vector3 direction;

    float disapearDuration = 1f;
    const float GRAVITY = 5f;

    public void Setup(int _comboCount, float _disapearDuration = 1f)
    {
        Destroy(gameObject, _disapearDuration);
        disapearDuration = _disapearDuration;
        if (comboText)
        {
            comboText.text = $"x{_comboCount}";
        }
        int _xRand = Random.Range(-30, 31);
        direction = new Vector3(_xRand / 100f, 2, 0);
        int _maxCombo = Mathf.Min(_comboCount, 5);

        comboText.fontSize = 24 + _maxCombo * 2;
        comboText.color = Color.Lerp(Color.white, Color.red, _maxCombo / 5f);
    }

    void Update()
    {
        if (disapearDuration > 0)
        {
            disapearDuration -= Time.deltaTime;

            comboText.alpha = Mathf.SmoothStep(1.0f, 0f, 1 - (disapearDuration / 1f));
            gameObject.transform.position += direction * Time.deltaTime;
            direction.y -= Time.deltaTime * GRAVITY;
        }
    }
}
