using UnityEngine;
using System.Collections.Generic;

public class MapBackgroundGenerator : MonoBehaviour
{
    private GlobalMapData globalMapData;

    public Sprite[] basicTileSprites;
    [HideInInspector]
    public List<List<GameObject>> mapTilesBackground;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GenerateNewBackgroundMap()
    {
        globalMapData = GetComponent<GlobalMapData>();
        if (globalMapData == null)
        {
            Debug.LogError("GlobalMapData component not found on the same GameObject. Please add a GlobalMapData component.");
            return;
        }
        GenerateBackgroundMap();
    }

    // Generates the map by creating tile GameObjects and assigning them random sprites
    void GenerateBackgroundMap()
    {
        // Create a parent GameObject to organize the tiles in the hierarchy
        GameObject _parent = new GameObject("MapBackground");
        _parent.transform.parent = this.transform;

        mapTilesBackground = new List<List<GameObject>>();
        for (int _i = 0; _i < globalMapData.mapSize.x; _i++)
        {
            mapTilesBackground.Add(new List<GameObject>());
            for (int _j = 0; _j < globalMapData.mapSize.y; _j++)
            {
                GenerateBackgroundTile(_i, _j, _parent.transform);
            }
        }
    }

    void GenerateBackgroundTile(int _i, int _j, Transform _parent)
    {
        GameObject _tile = new GameObject($"Tile_{_i}_{_j}");
        _tile.transform.position = globalMapData.GetTilePosition(_i, _j);
        _tile.transform.parent = _parent;
        SpriteRenderer _renderer = _tile.AddComponent<SpriteRenderer>();
        _renderer.sortingLayerName = "BackgroundTile";
        _renderer.sprite = GetRandomTileSprite();
        if (_renderer.sprite == null)
        {
            Debug.LogError($"Failed to assign a sprite to tile at position ({_i}, {_j}). Check the basicTileSprites array.");
        }
        mapTilesBackground[mapTilesBackground.Count - 1].Add(_tile);
    }

    // Returns a random sprite from the basicTileSprites array
    Sprite GetRandomTileSprite()
    {
        if (basicTileSprites == null || basicTileSprites.Length == 0)
        {
            Debug.LogError("Basic tile sprites array is empty or not assigned in the inspector.");
            return null;
        }
        int _index = Random.Range(0, basicTileSprites.Length);
        return basicTileSprites[_index];
    }
}
