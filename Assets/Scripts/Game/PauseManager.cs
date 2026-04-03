using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField] private GameObject pauseMenuGameobject;

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
            Time.timeScale = 1;
            pauseMenuGameobject.GetComponent<PauseScreenAnimation>().HidePauseScreen();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuGameobject.GetComponent<PauseScreenAnimation>().HidePauseScreen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
