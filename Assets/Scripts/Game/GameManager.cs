using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    // Reference to the GameObject that contains all the map generation components
    [SerializeField] private GameObject mapGenerator;
    private GlobalMapData globalMapData;
    private MapObstaclesGenerator mapObstaclesGenerator;
    private MapBackgroundGenerator mapBackgroundGenerator;
    private MapAppleGenerator mapAppleGenerator;

    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    [SerializeField] private GameObject camera;

    [HideInInspector]
    public bool isGameOver = false;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private int targetScore = 200;
    [Header("Audio Clips")]
    [SerializeField] private SnakeAudioClip gameOverAudioClip;
    [SerializeField] private SnakeAudioClip winAudioClip;

    private ZoomOnDeath zoomOnDeath;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (mapGenerator == null)
        {
            Debug.LogError("Map Generator GameObject is not assigned. Please assign a GameObject with MapObstaclesGenerator and MapBackgroundGenerator components.");
            return;
        }
        mapBackgroundGenerator = mapGenerator.GetComponent<MapBackgroundGenerator>();
        mapObstaclesGenerator = mapGenerator.GetComponent<MapObstaclesGenerator>();
        mapAppleGenerator = mapGenerator.GetComponent<MapAppleGenerator>();
        globalMapData = mapGenerator.GetComponent<GlobalMapData>();
        zoomOnDeath = camera.GetComponent<ZoomOnDeath>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RestartGame();
    }

    // Restart the game by resetting the game
    public void RestartGame()
    {
        isGameOver = false;
        if (gameOverUI != null)
        {
            GameOverScreen _gameOverScreen = gameOverUI.GetComponent<GameOverScreen>();
            if (_gameOverScreen != null)
            {
                _gameOverScreen.HideGameOverScreen();
            }
        }
        GenerateMaps();
        if (player != null)
        {
            Destroy(player);
        }
        player = CreatePlayerOnTheMap();
        if (player != null)
        {
            SetPositionPlayer(player);
        }
        if (camera != null)
        {
            camera.transform.rotation = Quaternion.Euler(0, 0, 0);
            camera.transform.position = new Vector3(globalMapData.mapSize.x / 2 - 0.5f, globalMapData.mapSize.y / 2 + 0.5f, camera.transform.position.z);
            if (zoomOnDeath != null)
            {
                zoomOnDeath.SetupOriginalPosition();
                zoomOnDeath.ResetZoom();
            }
        }
        if (ComboMananger.Instance != null)
            ComboMananger.Instance.Reset();
        ScoreManager.instance.ResetScore();
        ScoreManager.instance.SetTargetScore(targetScore);
        GameObject[] _particules = GameObject.FindGameObjectsWithTag("ParticulesEffect");
        foreach (GameObject _particule in _particules)
        {
            Destroy(_particule);
        }
    }

    // Generate new maps
    void GenerateMaps()
    {
        if (globalMapData == null)
        {
            Debug.LogError("GlobalMapData component not found on the mapGenerator GameObject. Please add a GlobalMapData component.");
            return;
        }
        globalMapData.GenerateNewMapPositions();
        if (mapBackgroundGenerator != null)
            mapBackgroundGenerator.GenerateNewBackgroundMap();
        if (mapObstaclesGenerator != null)
            mapObstaclesGenerator.GenerateNewObstaclesMap();
        if (mapAppleGenerator != null)
            mapAppleGenerator.GenerateNewApplesMap();
    }

    // Create a new player GameObject on the map at the center position
    GameObject CreatePlayerOnTheMap()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned. Please assign a player prefab in the inspector.");
            return null;
        }
        if (globalMapData == null)
        {
            Debug.LogError("GlobalMapData component not found on the mapGenerator GameObject. Please add a GlobalMapData component.");
            return null;
        }
        Vector3 _playerPosition = globalMapData.GetTilePosition(globalMapData.mapSize.x / 2, globalMapData.mapSize.y / 2);
        return Instantiate(playerPrefab, _playerPosition, Quaternion.identity);
    }

    // Set the player's position to the center of the map and reset its movement and global map data reference
    void SetPositionPlayer(GameObject _player)
    {
        if (_player == null)
        {
            Debug.LogError("Failed to instantiate player prefab. Please check the player prefab and ensure it is properly set up.");
            return;
        }
        MovementPlayer _movementPlayer = _player.GetComponent<MovementPlayer>();
        if (_movementPlayer == null)
        {
            Debug.LogError("MovementPlayer component not found on the player prefab. Please add a MovementPlayer component to the player prefab.");
            return;
        }
        _movementPlayer.ResetMovement(_player.transform.position);
        _movementPlayer.SetGlobalMapData(globalMapData);
    }

    // Handle the game over state
    public void GameOver()
    {
        if (isGameOver)
            return;
        isGameOver = true;
        bool _isWin = ScoreManager.instance.GetScore() >= targetScore;

        if (gameOverUI != null)
        {
            GameOverScreen _gameOverScreen = gameOverUI.GetComponent<GameOverScreen>();
            if (_gameOverScreen != null)
            {
                _gameOverScreen.ShowGameOverScreen(_isWin);
            }
        }
        if (!_isWin)
            SoundEffectManager.instance.PlayAudioSourceSetPitch(gameOverAudioClip);
        else
            SoundEffectManager.instance.PlayAudioSourceSetPitch(winAudioClip);

        if (zoomOnDeath != null)
        {
            zoomOnDeath.ZoomAtPos(player.transform.position);
        }
    }
}
