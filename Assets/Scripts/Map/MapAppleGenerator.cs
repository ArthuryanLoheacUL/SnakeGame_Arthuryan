using System.Collections.Generic;
using UnityEngine;

public class MapAppleGenerator : MonoBehaviour
{
    private GlobalMapData globalMapData;
    public Sprite appleTileSprite;
    private List<GameObject> apples;
    public int maxApples = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GenerateNewApplesMap()
    {
        globalMapData = GetComponent<GlobalMapData>();
        if (globalMapData == null)
        {
            Debug.LogError("GlobalMapData component not found on the same GameObject. Please add a GlobalMapData component.");
            return;
        }
        DestroyApples();
        apples = new List<GameObject>();
        GenerateApplesMap();
    }

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
        if (apples.Count < maxApples)
        {
            GenerateApplesMap();
        }
    }

    void GenerateApplesMap()
    {
        BodySnake _bodySnake = FindAnyObjectByType<BodySnake>();
        List<Vector2> _availablePositions = GetAvailablePositions(_bodySnake);

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

    List<Vector2> GetAvailablePositions(BodySnake _bodySnake)
    {
        List<Vector2> _availablePositions = new List<Vector2>();
        for (int _i = 0; _i < globalMapData.mapSize.x; _i++)
        {
            for (int _j = 0; _j < globalMapData.mapSize.y; _j++)
            {
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

    void GenerateAppleTile(int _i, int _j, Transform _parent)
    {
        GameObject _tile = new GameObject($"Apple_{_i}_{_j}");
        _tile.transform.position = new Vector2(_i, _j);
        _tile.transform.parent = _parent;
        SpriteRenderer _renderer = _tile.AddComponent<SpriteRenderer>();
        _renderer.sortingLayerName = "AppleTile";
        _renderer.sprite = appleTileSprite;
        if (_renderer.sprite == null)
        {
            Debug.LogError($"Failed to assign a sprite to tile at position ({_i}, {_j}). Check the basicTileSprites array.");
        }
        apples.Add(_tile);
    }

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

    public void RemoveAppleAtPosition(Vector2 _position)
    {
        for (int _i = 0; _i < apples.Count; _i++)
        {
            if (apples[_i].transform.position == new Vector3(_position.x, _position.y, 0))
            {
                Destroy(apples[_i]);
                apples.RemoveAt(_i);
                return;
            }
        }
    }
}
