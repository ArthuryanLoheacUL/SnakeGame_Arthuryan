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
    private Shader shaderDefault;

    public GameObject trailSmoke;

    // Reset the snake to its initial state, clearing all body parts and resetting the length and positions
    public void ResetSnake()
    {
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderDefault = Shader.Find("Sprites/Default");

        lengthSnake = startLengthSnake;
        positions.Clear();
        foreach (GameObject _bodyPart in bodyParts)
        {
            Destroy(_bodyPart);
        }
        bodyParts.Clear();
    }

    // Add a new position to the snake's body. Keeping a max number of positions based on the current length of the snake, and refreshing the body parts to match the new positions
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

    // Destroy all existing body part GameObjects and create new ones based on the current positions of the snake, setting the appropriate sprite for each part based on its position and direction
    void RefreshSnakeBody()
    {
        int _index = 0;

        foreach (Position _position in positions)
        {
            SpriteRenderer _renderer;
            if (_index < bodyParts.Count)
            {
                GameObject _bodyPart = bodyParts[_index];
                _bodyPart.transform.position = new Vector3(_position.position.x, _position.position.y, 0);
                _renderer = _bodyPart.GetComponent<SpriteRenderer>();
            }
            else
            {
                GameObject _newBodyPart = new GameObject("BodyPart");
                _newBodyPart.transform.position = new Vector3(_position.position.x, _position.position.y, 0);
                _renderer = _newBodyPart.AddComponent<SpriteRenderer>();
                _renderer.sortingLayerName = "Player";
                _newBodyPart.transform.parent = transform;
                bodyParts.Add(_newBodyPart);
            }
            _renderer.sprite = GetSprite(_index, _position.direction, positions);
            if (impactWhiteDuration > 0)
            {
                _renderer.material.shader = shaderGUItext;
            }
            else
            {
                _renderer.material.shader = shaderDefault;
            }
            _index++;
        }
    }

    // Get the appropriate sprite for the tail based on the direction of the second to last body part
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

    // Get the appropriate sprite for a body part based on the positions and directions of the previous and next body parts,
    // determining if it's a straight segment or a corner and returning the corresponding sprite
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

    // Get the appropriate sprite for a body part based on its index in the positions list, determining if it's the head, tail, or a body segment and returning the corresponding sprite
    Sprite GetSprite(int _index, Vector2 _direction, List<Position> _positions = null)
    {
        if (_index == positions.Count - 1)
            return GetSpriteFromHead(_direction);
        else if (_index == 0)
            return GetSpriteFromDirection(tailSprite, _positions[1].direction);
        return GetSpriteForBody(_positions[_index], _positions[_index - 1], _positions[_index + 1]);
    }

    // Increase the length of the snake by incrementing the lengthSnake variable, allowing the snake to grow when it eats an apple
    public void IncreaseLengthSnake()
    {
        lengthSnake++;
    }

    // Check if a given position is occupied by any part of the snake's body (excluding the tail) by iterating through the positions list and comparing each position with the given position
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

    // Update is called once per frame
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

        // Update the impact white effect duration, resetting it to 0 when it expires
        if (impactWhiteDuration > 0)
        {
            impactWhiteDuration -= Time.deltaTime;
            if (impactWhiteDuration <= 0)
            {
                impactWhiteDuration = 0;
            }
        }
    }

    // Get the appropriate head sprite based on the current direction of the head and the current head state (basic, eating, or dead), returning the corresponding sprite for the head
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

    // Get the appropriate head sprite based on the current head state (basic, eating, or dead) and the corresponding sprites for each state
    // returning the correct sprite for the head based on its current state and animation frame
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

    // Set the current head state of the snake (basic, eating, or dead) and reset the head animation frame to 0 to start the new animation sequence
    public void SetHeadState(HeadState _state)
    {
        currentHeadState = _state;
        currentHeadFrame = 0;
    }

    // Trigger the impact with wall effect
    public void ImpactWall()
    {
        SetHeadState(HeadState.Dead);
        impactWhiteDuration = 0.12f;
    }
}
