using UnityEngine;
using System.Collections.Generic;

public class GlobalMapData : MonoBehaviour
{
    public Vector2Int mapSize = new Vector2Int(10, 10);
    [SerializeField] [Range(0f, 1f)]
    private float obstacleDensity = 0.1f;
    private List<List<Vector2>> mapPositions;

    private MapAppleGenerator mapAppleGenerator;
    private MapObstaclesGenerator mapObstaclesGenerator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GenerateNewMapPositions()
    {
        GenerateMapPositions();
    }

    private void Start()
    {
        mapAppleGenerator = GetComponent<MapAppleGenerator>();
        mapObstaclesGenerator = GetComponent<MapObstaclesGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMapPositions()
    {
        mapPositions = new List<List<Vector2>>();
        for (int _i = 0; _i < mapSize.x; _i++)
        {
            mapPositions.Add(new List<Vector2>());
            for (int _j = 0; _j < mapSize.y; _j++)
            {
                Vector2 _position = new Vector2(_i, _j);
                mapPositions[mapPositions.Count - 1].Add(_position);
            }
        }
    }

    public Vector2 GetTilePosition(int _x, int _y)
    {
        if (_x < 0 || _x >= mapSize.x || _y < 0 || _y >= mapSize.y)
        {
            Debug.LogError($"Tile coordinates ({_x}, {_y}) are out of bounds. Map size is ({mapSize.x}, {mapSize.y}).");
            return Vector2.zero;
        }
        return mapPositions[_x][_y];
    }

    public bool IsTileExistingAtPosition(Vector2 _position)
    {
        if (_position.x < 0 || _position.x >= mapSize.x || _position.y < 0 || _position.y >= mapSize.y)
        {
            return false;
        }
        return true;
    }

    public bool IsObstacleTileAtPosition(Vector2 _position)
    {
        if (mapObstaclesGenerator != null)
        {
            return mapObstaclesGenerator.IsObstacleTileAtPosition(_position);
        }
        return false;
    }

    public bool IsAppleTileAtPosition(Vector2 _position)
    {
        if (mapAppleGenerator != null)
        {
            return mapAppleGenerator.IsAppleTileAtPosition(_position);
        }
        return false;
    }

    public void RemoveAppleAtPosition(Vector2 _position)
    {
        if (mapAppleGenerator != null)
        {
            mapAppleGenerator.RemoveAppleAtPosition(_position);
        }
    }

    public float GetObstacleDensity()
    {
        return obstacleDensity;
    }
}
