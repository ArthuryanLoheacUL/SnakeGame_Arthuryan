using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float length;
    [SerializeField] private Vector2 direction;

    private Vector2 originalPos = Vector2.zero;

    void Start()
    {
        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }   

    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(
            originalPos.x + Mathf.Sin(Time.realtimeSinceStartup * speed) * length * direction.x,
            originalPos.y + Mathf.Sin(Time.realtimeSinceStartup * speed) * length * direction.y);
    }
}
