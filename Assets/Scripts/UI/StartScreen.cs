using TMPro;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    private Vector2 initialPos;
    private Vector2 targetScale;
    private Vector2 targetPos;

    private bool isMoving = false;
    private float speedAnimation = 8f;
    private float previousTime = 0f;
    private float thisTime = 0f;

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
            Vector2 _scaleSpeed = _currentScale.y < targetScale.y ?
                new Vector2(speedAnimation, speedAnimation * 2) : new Vector2(speedAnimation * 2, speedAnimation);
            Vector2 _newScale = new Vector2(
                Mathf.Lerp(_currentScale.x, targetScale.x, Time.deltaTime * _scaleSpeed.x),
                Mathf.Lerp(_currentScale.y, targetScale.y, Time.deltaTime * _scaleSpeed.y)
            );
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
