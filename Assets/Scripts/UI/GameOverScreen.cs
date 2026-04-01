using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public string gameOverMessage = "Game Over!";
    public string winMessage = "You Win!";

    // Display the game over screen with the appropriate message based on win or lose
    public void ShowGameOverScreen(bool _isWin)
    {
        string _message = _isWin ? winMessage : gameOverMessage;
        gameOverText.text = _message;
    }
}
