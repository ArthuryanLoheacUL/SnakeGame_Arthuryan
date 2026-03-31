using System.Collections.Generic;
using UnityEngine;

public class MapObstaclesGenerator : MonoBehaviour
{
    private GlobalMapData globalMapData;
    public Sprite[] obstaclesTileSprites;
    public List<GameObject> mapTilesObstacles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GenerateNewObstaclesMap()
    {
        globalMapData = GetComponent<GlobalMapData>();
        if (globalMapData == null)
        {
            Debug.LogError("GlobalMapData component not found on the same GameObject. Please add a GlobalMapData component.");
            return;
        }
        GenerateObstaclesMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateObstaclesMap()
    {
        // Create a parent GameObject to organize the tiles in the hierarchy
        GameObject _parent = new GameObject("MapObstacles");
        _parent.transform.parent = this.transform;

        int _totalTiles = globalMapData.mapSize.x * globalMapData.mapSize.y;
        int _obstacleCount = Mathf.RoundToInt(_totalTiles * globalMapData.GetObstacleDensity());

        mapTilesObstacles = new List<GameObject>();
        List<Vector2> _availableSpaces = GetAvailableSpaces();

        for (int _i = 0; _i < _obstacleCount; _i++)
        {
            if (_availableSpaces.Count == 0)
            {
                Debug.LogWarning("No more available spaces to place obstacles.");
                break;
            }
            int _availableIndex = Random.Range(0, _availableSpaces.Count);
            Vector2 _position = _availableSpaces[_availableIndex];
            GenerateObstacleTile((int)_position.x, (int)_position.y, _parent.transform);
            _availableSpaces.RemoveAt(_availableIndex);
        }
    }

    List<Vector2> GetAvailableSpaces()
    {
        List<Vector2> _availableSpaces = new List<Vector2>();
        for (int _i = 0; _i < globalMapData.mapSize.x; _i++)
        {
            for (int _j = 0; _j < globalMapData.mapSize.y; _j++)
            {
                _availableSpaces.Add(new Vector2(_i, _j));
            }
        }
        return _availableSpaces;
    }

    void GenerateObstacleTile(int _i, int _j, Transform _parent)
    {
        GameObject _tile = new GameObject($"Tile_{_i}_{_j}");
        _tile.transform.position = new Vector3(_i, _j, 0);
        _tile.transform.parent = _parent;
        SpriteRenderer _renderer = _tile.AddComponent<SpriteRenderer>();
        _renderer.sortingLayerName = "ObstacleTile";
        _renderer.sprite = GetRandomTileSprite();
        if (_renderer.sprite == null)
        {
            Debug.LogError($"Failed to assign a sprite to tile at position ({_i}, {_j}). Check the basicTileSprites array.");
        }
        mapTilesObstacles.Add(_tile);
    }

    // Returns a random sprite from the basicTileSprites array
    Sprite GetRandomTileSprite()
    {
        if (obstaclesTileSprites == null || obstaclesTileSprites.Length == 0)
        {
            Debug.LogError("Basic tile sprites array is empty or not assigned in the inspector.");
            return null;
        }
        int _index = Random.Range(0, obstaclesTileSprites.Length);
        return obstaclesTileSprites[_index];
    }

    public bool IsObstacleTileAtPosition(Vector2 _position)
    {
        foreach (GameObject _tile in mapTilesObstacles)
        {
            if (_tile.transform.position == new Vector3(_position.x, _position.y, 0))
            {
                return true;
            }
        }
        return false;
    }
}
