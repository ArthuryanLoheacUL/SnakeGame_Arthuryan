using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public void LoadScene(string _sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }
}
