using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        GameManager.instance.RestartGame();
    }
}
