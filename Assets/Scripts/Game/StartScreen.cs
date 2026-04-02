using TMPro;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    private Vector2 targetScale;
    private Vector2 targetPos;
    private Vector2 initialPos;
    bool isMoving = false;

    float speedAnimation = 8f;

    float previousTime = 0f;
    float thisTime = 0f;

    void Start()
    {
        Time.timeScale = 0f;

        initialPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    public void HideStartScreen()
    {
        targetPos = new Vector2(initialPos.x, initialPos.y - 300);
        targetScale = Vector2.zero;
        isMoving = true;
        previousTime = Time.realtimeSinceStartup;
        Time.timeScale = 1f;
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
