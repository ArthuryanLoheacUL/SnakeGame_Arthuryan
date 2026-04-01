using UnityEngine;

public class RestartButton : MonoBehaviour
{
    // Restart the game when the button is clicked
    public void RestartGame()
    {
        GameManager.instance.RestartGame();
    }
}
