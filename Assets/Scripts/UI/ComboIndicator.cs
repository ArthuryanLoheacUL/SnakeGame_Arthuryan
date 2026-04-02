using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComboIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject fireEffect;

    private Vector3 direction;
    private float disapearDuration = 1f;
    private const float GRAVITY = 5f;

    public void Setup(int _comboCount, float _disapearDuration = 1f)
    {
        Destroy(gameObject, _disapearDuration);
        disapearDuration = _disapearDuration;
        if (comboText)
        {
            comboText.text = $"+{_comboCount}";
        }
        int _xRand = Random.Range(-30, 31);
        direction = new Vector3(_xRand / 100f, 2, 0);
        int _maxCombo = Mathf.Min(_comboCount, 5);

        comboText.fontSize = 24 + _maxCombo * 3;
        fireEffect.SetActive(_comboCount >= 5);
    }

    void Update()
    {
        if (disapearDuration > 0)
        {
            disapearDuration -= Time.deltaTime;

            comboText.alpha = Mathf.SmoothStep(1.0f, 0f, 1 - (disapearDuration / 1f));
            if (fireEffect.transform.childCount > 0 && fireEffect.transform.GetChild(0).GetComponent<Image>())
                fireEffect.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, comboText.alpha/ 1.5f);
            gameObject.transform.position += direction * Time.deltaTime;
            direction.y -= Time.deltaTime * GRAVITY;
        }
    }
}
