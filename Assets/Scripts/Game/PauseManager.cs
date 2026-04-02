using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField]
    private GameObject pauseMenuGameobject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (pauseMenuGameobject)
        {
            pauseMenuGameobject.SetActive(false);
        }
    }

    public void TogglePause()
    {
        bool _settingPause = Time.timeScale > 0;
        if (_settingPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        pauseMenuGameobject.SetActive(_settingPause);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuGameobject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
