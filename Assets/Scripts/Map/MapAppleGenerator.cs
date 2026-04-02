using System.Collections.Generic;
using UnityEngine;

public class MapAppleGenerator : MonoBehaviour
{
    private GlobalMapData globalMapData;
    public Sprite appleTileSprite;
    public GameObject appleTilePrefab;
    private List<GameObject> apples;
    private int maxApples = 1;
    public int startApples = 1;
    private int nextAppleScoreThreshold = 10;
    private int nextAppleScore = 0;

    // Clear the previous map and generate a new one with new apple positions
    public void GenerateNewApplesMap()
    {
        nextAppleScoreThreshold = 10;
        nextAppleScore = nextAppleScoreThreshold;
        maxApples = startApples;
        globalMapData = GetComponent<GlobalMapData>();
        if (globalMapData == null)
        {
            Debug.LogError("GlobalMapData component not found on the same GameObject. Please add a GlobalMapData component.");
            return;
        }
        DestroyApples();
        apples = new List<GameObject>();
        GenerateApplesMap(true);
    }

    // Destroy all existing apple GameObjects and clear the apples list
    void DestroyApples()
    {
        if (apples != null)
        {
            foreach (GameObject _tile in apples)
            {
                Destroy(_tile);
            }
            apples.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.instance != null)
        {
            if (ScoreManager.instance.GetScore() >= nextAppleScore)
            {
                nextAppleScoreThreshold += 20;
                nextAppleScore += nextAppleScoreThreshold;
                maxApples++;
            }
        }
        if (apples.Count < maxApples)
        {
            GenerateApplesMap(false);
        }
    }

    // Generate new apple tiles at random available positions on the map, ensuring they do not overlap with the snake or obstacles
    void GenerateApplesMap(bool _firstApples)
    {
        BodySnake _bodySnake = FindAnyObjectByType<BodySnake>();
        List<Vector2> _availablePositions = GetAvailablePositions(_bodySnake, _firstApples);

        for (int _i = apples.Count; _i < maxApples; _i++)
        {
            if (_availablePositions.Count == 0)
            {
                return;
            }

            int _availableIndex = Random.Range(0, _availablePositions.Count);
            Vector2 _position = _availablePositions[_availableIndex];

            GenerateAppleTile((int)_position.x, (int)_position.y, this.transform);
            _availablePositions.RemoveAt(_availableIndex);
        }
    }

    // Get a list of available positions on the map where an apple can be placed, ensuring they do not overlap with the snake or obstacles
    List<Vector2> GetAvailablePositions(BodySnake _bodySnake, bool _firstApples)
    {
        List<Vector2> _availablePositions = new List<Vector2>();
        for (int _i = 0; _i < globalMapData.mapSize.x; _i++)
        {
            for (int _j = 0; _j < globalMapData.mapSize.y; _j++)
            {
                if (_firstApples && _j == globalMapData.mapSize.y / 2)
                {
                    continue; // Skip the middle row for the first apple generation to ensure no immediate collision with the snake's starting position
                }

                Vector2 _position = globalMapData.GetTilePosition(_i, _j);
                bool _isOnSnake = _bodySnake != null && _bodySnake.IsPositionOnSnake(_position);
                if (!IsAppleTileAtPosition(_position) && !globalMapData.IsObstacleTileAtPosition(_position) && !_isOnSnake)
                {
                    _availablePositions.Add(_position);
                }
            }
        }
        return _availablePositions;
    }

    // Generate a new apple tile GameObject at the specified position and parent it to the MapAppleGenerator's transform
    void GenerateAppleTile(int _i, int _j, Transform _parent)
    {
        GameObject _tile = Instantiate(appleTilePrefab);
        _tile.transform.position = new Vector2(_i, _j);
        _tile.transform.parent = _parent;
        SpriteRenderer _renderer = _tile.GetComponent<SpriteRenderer>();
        _renderer.sortingLayerName = "AppleTile";
        _renderer.sprite = appleTileSprite;
        if (_renderer.sprite == null)
        {
            Debug.LogError($"Failed to assign a sprite to tile at position ({_i}, {_j}). Check the basicTileSprites array.");
        }
        apples.Add(_tile);
    }

    // Check if there is an apple tile at the specified position by comparing the position of each apple tile with the given position
    public bool IsAppleTileAtPosition(Vector2 _position)
    {
        foreach (GameObject _tile in apples)
        {
            if (_tile.transform.position == new Vector3(_position.x, _position.y, 0))
            {
                return true;
            }
        }
        return false;
    }

    // Remove the apple tile at the specified position by finding the apple tile with the matching position, destroying it, and removing it from the apples list
    public void RemoveAppleAtPosition(Vector2 _position)
    {
        for (int _i = 0; _i < apples.Count; _i++)
        {
            if (apples[_i].transform.position == new Vector3(_position.x, _position.y, 0))
            {
                apples[_i].GetComponent<SplashOnDestroy>()?.Splash();
                Destroy(apples[_i]);
                apples.RemoveAt(_i);
                return;
            }
        }
    }
}
