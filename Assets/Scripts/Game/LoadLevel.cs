using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public void LoadScene(string _sceneName)
    {
        LevelLoader.instance.LoadLevel(_sceneName);
    }
}
