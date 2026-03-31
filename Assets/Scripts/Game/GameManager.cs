using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    // Reference to the GameObject that contains all the map generation components
    public GameObject mapGenerator;
    private GlobalMapData globalMapData;
    private MapObstaclesGenerator mapObstaclesGenerator;
    private MapBackgroundGenerator mapBackgroundGenerator;
    private MapAppleGenerator mapAppleGenerator;

    public GameObject playerPrefab;
    public GameObject camera;

    [HideInInspector]
    public bool isGameOver = false;
    public GameObject gameOverUI;

    private GameObject player;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mapGenerator == null)
        {
            Debug.LogError("Map Generator GameObject is not assigned. Please assign a GameObject with MapObstaclesGenerator and MapBackgroundGenerator components.");
            return;
        }
        mapBackgroundGenerator = mapGenerator.GetComponent<MapBackgroundGenerator>();
        mapObstaclesGenerator = mapGenerator.GetComponent<MapObstaclesGenerator>();
        mapAppleGenerator = mapGenerator.GetComponent<MapAppleGenerator>();
        globalMapData = mapGenerator.GetComponent<GlobalMapData>();
        RestartGame();
    }

    public void RestartGame()
    {
        isGameOver = false;
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
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
            camera.transform.position = new Vector3(globalMapData.mapSize.x / 2 - 0.5f, globalMapData.mapSize.y / 2 + 0.25f, camera.transform.position.z);
        }
        ScoreManager.instance.ResetScore();
    }

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

    public void GameOver()
    {
        isGameOver = true;
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }
}
