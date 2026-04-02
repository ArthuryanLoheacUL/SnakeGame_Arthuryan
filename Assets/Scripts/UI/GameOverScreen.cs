using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private string gameOverMessage = "Game Over!";
    [SerializeField] private string winMessage = "You Win!";
    private Vector2 targetScale;
    private Vector2 targetPos;
    private Vector2 initialPos;
    private Vector2 initialScale;
    private bool isMoving = false;

    private float speedAnimation = 8f;

    [SerializeField] private float delayBeforeShowing = 0.5f;
    [SerializeField] private GameObject confettisPrefab;

    void Start()
    {
        initialPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
        initialScale = gameObject.GetComponent<RectTransform>().localScale;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(initialPos.x, initialPos.y - 300);
        gameObject.GetComponent<RectTransform>().localScale = Vector2.zero;
        gameObject.SetActive(false);
    }

    // Display the Game Over screen
    public void ShowGameOverScreen(bool _isWin)
    {
        isMoving = false;
        gameObject.SetActive(true);
        targetPos = initialPos;
        if (gameOverText)
        {
            gameOverText.text = _isWin ? winMessage : gameOverMessage;
        }
        targetScale = initialScale;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(initialPos.x, initialPos.y - 300);
        gameObject.GetComponent<RectTransform>().localScale = Vector2.zero;
        StartCoroutine(ShowGameOverScreenWithDelay(delayBeforeShowing, _isWin));
    }

    System.Collections.IEnumerator ShowGameOverScreenWithDelay(float _delay, bool _isWin = false)
    {
        yield return new WaitForSeconds(_delay);
        isMoving = true;
        if (_isWin && confettisPrefab)
        {
            GameObject _confetis = Instantiate(confettisPrefab, transform.position, Quaternion.identity, transform.parent);
            _confetis.GetComponent<RectTransform>().anchoredPosition = targetPos;
        }
    }

    public void HideGameOverScreen()
    {
        targetPos = new Vector2(initialPos.x, initialPos.y - 300);
        targetScale = Vector2.zero;
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            Vector2 _currentPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
            Vector2 _newPos = Vector2.Lerp(_currentPos, targetPos, Time.deltaTime * speedAnimation);
            gameObject.GetComponent<RectTransform>().anchoredPosition = _newPos;

            Vector2 _currentScale = gameObject.GetComponent<RectTransform>().localScale;
            Vector2 _newScale = Vector2.Lerp(_currentScale, targetScale, Time.deltaTime * speedAnimation);
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
