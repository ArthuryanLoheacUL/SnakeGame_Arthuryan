using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

struct Position
{
    public Vector2 direction;
    public Vector2 position;
}

[System.Serializable]
public struct SnakeSprite
{
    public Sprite left;
    public Sprite right;
    public Sprite up;
    public Sprite down;
}

[System.Serializable]
public struct HeadSnakePart
{
    public Sprite basic;
    public Sprite[] eating;
    public Sprite[] dead;
}

[System.Serializable]
public struct HeadSnake
{
    public HeadSnakePart left;
    public HeadSnakePart right;
    public HeadSnakePart up;
    public HeadSnakePart down;
    public float frameDurationEating;
    public float frameDurationDead;
}

[System.Serializable]
public struct SnakeSpriteBody
{
    public Sprite vertical;
    public Sprite horizontal;
    public Sprite leftUp;
    public Sprite rightUp;
    public Sprite leftDown;
    public Sprite rightDown;
}

public class BodySnake : MonoBehaviour
{
    public int startLengthSnake = 2;
    private int lengthSnake = 2;
    private List<Position> positions = new List<Position>();
    private List<GameObject> bodyParts = new List<GameObject>();

    public SnakeSpriteBody bodySprite;
    public HeadSnake headSprite;
    public SnakeSprite tailSprite;

    private float headAnimationTimer;
    private int currentHeadFrame;

    public enum HeadState { Basic, Eating, Dead }
    public HeadState currentHeadState = HeadState.Basic;

    private float impactWhiteDuration = 0.0f;
    private Shader shaderGUItext;

    public GameObject trailSmoke;


    public void ResetSnake()
    {
        shaderGUItext = Shader.Find("GUI/Text Shader");

        lengthSnake = startLengthSnake;
        positions.Clear();
        foreach (GameObject _bodyPart in bodyParts)
        {
            Destroy(_bodyPart);
        }
        bodyParts.Clear();
    }

    public void AddPositionSnake(Vector2 _position, Vector2 _direction)
    {
        Position _partPos = new Position();
        _partPos.position = _position;
        _partPos.direction = _direction;
        positions.Add(_partPos);
        if (positions.Count > lengthSnake)
        {
            positions.RemoveAt(0);
        }
        RefreshSnakeBody();
        if (trailSmoke != null)
        {
            trailSmoke.transform.position = new Vector3(positions[0].position.x, positions[0].position.y, 0);
            trailSmoke.transform.rotation = Quaternion.LookRotation(Vector3.back, positions[0].direction);
        }
    }

    void RefreshSnakeBody()
    {
        foreach (GameObject _bodyPart in bodyParts)
        {
            Destroy(_bodyPart);
        }
        int _index = 0;

        foreach (Position _position in positions)
        {
            GameObject _bodyPart = new GameObject("BodyPart");
            _bodyPart.transform.position = new Vector3(_position.position.x, _position.position.y, 0);
            SpriteRenderer _renderer = _bodyPart.AddComponent<SpriteRenderer>();
            _renderer.sprite = GetSprite(_index, _position.direction, positions);
            _renderer.sortingLayerName = "Player";
            if (impactWhiteDuration > 0)
            {
                _renderer.material.shader = shaderGUItext;
                _renderer.color = Color.white;
            }
            _bodyPart.transform.parent = transform;
            bodyParts.Add(_bodyPart);
            _index++;
        }
    }

    Sprite GetSpriteFromDirection(SnakeSprite _sprites, Vector2 _direction)
    {
        if (_direction == Vector2.up)
            return _sprites.up;
        else if (_direction == Vector2.down)
            return _sprites.down;
        else if (_direction == Vector2.left)
            return _sprites.left;
        else if (_direction == Vector2.right)
            return _sprites.right;
        return null;
    }

    Sprite GetSpriteForBody(Position _current, Position _previous, Position _next)
    {
        if (_current.position.x == _previous.position.x && _current.position.x == _next.position.x)
            return bodySprite.vertical;
        else if (_current.position.y == _previous.position.y && _current.position.y == _next.position.y)
            return bodySprite.horizontal;
        else if (_previous.position.x < _current.position.x && _next.position.y > _current.position.y ||
                 _next.position.x < _current.position.x && _previous.position.y > _current.position.y)
            return bodySprite.leftUp;
        else if (_previous.position.x > _current.position.x && _next.position.y > _current.position.y ||
                _next.position.x > _current.position.x && _previous.position.y > _current.position.y)
            return bodySprite.rightUp;
        else if (_previous.position.x < _current.position.x && _next.position.y < _current.position.y ||
                _next.position.x < _current.position.x && _previous.position.y < _current.position.y)
            return bodySprite.leftDown;
        else if (_previous.position.x > _current.position.x && _next.position.y < _current.position.y ||
                _next.position.x > _current.position.x && _previous.position.y < _current.position.y)
            return bodySprite.rightDown;
        return null;
    }

    Sprite GetSprite(int _index, Vector2 _direction, List<Position> _positions = null)
    {
        if (_index == positions.Count - 1)
            return GetSpriteFromHead(_direction);
        else if (_index == 0)
            return GetSpriteFromDirection(tailSprite, _positions[1].direction);
        return GetSpriteForBody(_positions[_index], _positions[_index - 1], _positions[_index + 1]);
    }

    public void IncreaseLengthSnake()
    {
        lengthSnake++;
    }

    public bool IsPositionOnSnake(Vector2 _position)
    {
        foreach (Position _pos in positions)
        {
            if (_pos.position == _position && _pos.position != positions[0].position)
            {
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        // Update the head sprite based on the current direction
        headAnimationTimer += Time.deltaTime;
        if (headAnimationTimer >= headSprite.frameDurationEating && currentHeadState == HeadState.Eating ||
            headAnimationTimer >= headSprite.frameDurationDead && currentHeadState == HeadState.Dead)
        {
            headAnimationTimer = 0f;
            currentHeadFrame++;
            RefreshSnakeBody();
        }

        if (impactWhiteDuration > 0)
        {
            impactWhiteDuration -= Time.deltaTime;
            if (impactWhiteDuration <= 0)
            {
                impactWhiteDuration = 0;
            }
        }
    }

    Sprite GetSpriteFromHead(Vector2 _direction)
    {
        switch (_direction.x, _direction.y)
        {
            case (0, 1):
                return GetSpriteFromHeadDirection(headSprite.up);
            case (0, -1):
                return GetSpriteFromHeadDirection(headSprite.down);
            case (-1, 0):
                return GetSpriteFromHeadDirection(headSprite.left);
            case (1, 0):
                return GetSpriteFromHeadDirection(headSprite.right);
            default:
                return null;
        }
    }

    Sprite GetSpriteFromHeadDirection(HeadSnakePart _snakePart)
    {
        switch (currentHeadState)
        {
            case HeadState.Basic:
                return _snakePart.basic;
            case HeadState.Eating:
                if (currentHeadFrame >= _snakePart.eating.Length)
                {
                    SetHeadState(HeadState.Basic);
                    return _snakePart.basic;
                }
                return _snakePart.eating[currentHeadFrame];
            case HeadState.Dead:
                if (currentHeadFrame >= _snakePart.dead.Length)
                {
                    currentHeadFrame = 0;
                }
                return _snakePart.dead[currentHeadFrame];
            default:
                return _snakePart.basic;
        }
    }

    public void SetHeadState(HeadState _state)
    {
        currentHeadState = _state;
        currentHeadFrame = 0;
    }

    public void ImpactWall()
    {
        SetHeadState(HeadState.Dead);
        impactWhiteDuration = 0.12f;
    }
}
