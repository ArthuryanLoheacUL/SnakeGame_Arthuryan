using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField] private GameObject pauseMenuGameobject;
    bool isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (pauseMenuGameobject)
        {
            pauseMenuGameobject.GetComponent<PauseScreenAnimation>().HidePauseScreen();
        }
    }

    public void TogglePause()
    {
        bool _settingPause = Time.timeScale > 0;
        if (_settingPause)
        {
            Time.timeScale = 0;
            pauseMenuGameobject.GetComponent<PauseScreenAnimation>().ShowPauseScreen();
        }
        else
        {
            Resume();
        }
        isPaused = _settingPause;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuGameobject.GetComponent<PauseScreenAnimation>().HidePauseScreen();
        isPaused = false;
    }

    void Update()
    {
        if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.instance.isGameOver && (Time.timeScale > 0 && !isPaused))
        {
            TogglePause();
        }
    }
}
