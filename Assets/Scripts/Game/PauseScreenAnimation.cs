using TMPro;
using UnityEngine;

public class PauseScreenAnimation : MonoBehaviour
{
    private Vector2 targetScale;
    private Vector2 targetPos;
    private Vector2 initialPos;
    private Vector2 initialScale;

    private bool isMoving = false;
    private float speedAnimation = 8f;
    private float previousTime = 0f;
    private float thisTime = 0f;

    void Start()
    {
        initialPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
        initialScale = gameObject.GetComponent<RectTransform>().localScale;
        gameObject.SetActive(false);
    }

    // Display the pause screen
    public void ShowPauseScreen()
    {
        gameObject.SetActive(true);
        targetPos = initialPos;
        targetScale = initialScale;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(initialPos.x, initialPos.y - 300);
        gameObject.GetComponent<RectTransform>().localScale = Vector2.zero;
        isMoving = true;
        previousTime = Time.realtimeSinceStartup;
    }

    public void HidePauseScreen()
    {
        targetPos = new Vector2(initialPos.x, initialPos.y - 300);
        targetScale = Vector2.zero;
        isMoving = true;
        previousTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (isMoving)
        {
            thisTime = Time.realtimeSinceStartup;
            float _deltaTime = thisTime - previousTime;
            previousTime = thisTime;

            Vector2 _currentPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
            Vector2 _newPos = Vector2.Lerp(_currentPos, targetPos, _deltaTime * speedAnimation);
            gameObject.GetComponent<RectTransform>().anchoredPosition = _newPos;

            Vector2 _currentScale = gameObject.GetComponent<RectTransform>().localScale;
            Vector2 _newScale = Vector2.Lerp(_currentScale, targetScale, _deltaTime * speedAnimation);
            gameObject.GetComponent<RectTransform>().localScale = _newScale;

            if (Vector2.Distance(_newPos, targetPos) < 0.1f)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition = targetPos;
                gameObject.GetComponent<RectTransform>().localScale = targetScale;
                isMoving = false;
                if (targetScale == Vector2.zero)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
