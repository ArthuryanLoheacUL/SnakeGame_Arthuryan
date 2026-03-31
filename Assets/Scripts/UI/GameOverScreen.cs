using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public string gameOverMessage = "Game Over!";
    public string winMessage = "You Win!";

    public void ShowGameOverScreen(bool _isWin)
    {
        string _message = _isWin ? winMessage : gameOverMessage;
        gameOverText.text = _message;
    }
}
