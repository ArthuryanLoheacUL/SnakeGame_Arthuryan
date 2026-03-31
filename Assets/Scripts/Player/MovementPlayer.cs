using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MovementPlayer : MonoBehaviour
{
    private Vector2 pos;
    public float timerBetweenMoves = 0.25f;
    private float timer;

    private Vector2 lastDirection = Vector2.right;
    private BodySnake bodySnake;

    GlobalMapData globalMapData;

    List<Vector2> inputBuffer = new List<Vector2>();
    const int MAX_INPUT_BUFFER_SIZE = 4;

    // Audio
    private SnakeAudio snakeAudio;

    void Start()
    {
        snakeAudio = GetComponent<SnakeAudio>();
    }

    public void ResetMovement(Vector2 _pos)
    {
        bodySnake = GetComponent<BodySnake>();
        int _dir = Random.Range(0, 2);
        lastDirection = _dir switch
        {
            0 => Vector2.right,
            _ => Vector2.left
        };


        if (bodySnake != null)
        {
            bodySnake.ResetSnake();
        }
        for (int _i = bodySnake.startLengthSnake - 1; _i >= 0; _i--)
            SetPosition(_pos - (lastDirection * _i));
    }

    public void SetGlobalMapData(GlobalMapData _globalMapData)
    {
        globalMapData = _globalMapData;
    }

    public void SetPosition(Vector2 _position)
    {
        pos = _position;
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        if (bodySnake != null)
        {
            bodySnake.AddPositionSnake(pos, lastDirection);
        }
    }

    void Update()
    {
        if (GameManager.instance.isGameOver)
            return;
        UpdateMovement();
    }

    void UpdateMovement()
    {
        timer += Time.deltaTime;
        if (timer >= timerBetweenMoves)
        {
            if (inputBuffer.Count > 0)
            {
                lastDirection = inputBuffer[0];
                inputBuffer.RemoveAt(0);
            }

            if (CheckCollisionNextPosition(pos + lastDirection))
            {
                if (CheckAppleNextPosition(pos + lastDirection) && bodySnake != null)
                {
                    EatApple();
                }
                SetPosition(pos + lastDirection);
            } else
            {
                HitWall();
            }
            timer = 0f;
        }
    }

    void HitWall()
    {
        if (snakeAudio != null)
        {
            snakeAudio.PlayHitWallSound();
        }
        GameManager.instance.GameOver();
        ShakeCameraManager.instance.ShakeCamera(0.15f, 0.15f, lastDirection);
        if (bodySnake != null)
        {
            bodySnake.SetHeadState(BodySnake.HeadState.Dead);
        }
    }

    void EatApple()
    {
        globalMapData.RemoveAppleAtPosition(pos + lastDirection);
        if (bodySnake != null)
        {
            bodySnake.IncreaseLengthSnake();
            bodySnake.SetHeadState(BodySnake.HeadState.Eating);
        }
        ScoreManager.instance.AddScore(50);
        if (snakeAudio != null)
        {
            snakeAudio.PlayAppleEatSound();
        }
    }

    bool CheckAppleNextPosition(Vector2 _position)
    {
        if (globalMapData != null)
        {
            return globalMapData.IsAppleTileAtPosition(_position);
        }
        return false;
    }

    bool CheckCollisionNextPosition(Vector2 _position)
    {
        if (globalMapData != null)
        {
            if (!globalMapData.IsTileExistingAtPosition(_position) ||
                globalMapData.IsObstacleTileAtPosition(_position) ||
                !CheckCollisionNextPositionWithBodySnake(_position))
            {
                return false;
            }
        }
        return true;
    }

    bool CheckCollisionNextPositionWithBodySnake(Vector2 _position)
    {
        if (bodySnake != null)
        {
            return !bodySnake.IsPositionOnSnake(_position);
        }
        return true;
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        Vector2 _input = _context.ReadValue<Vector2>();
        Vector2 _newTargetDirection = Vector2.zero;
        if (_input.x != 0)
        {
            _newTargetDirection = new Vector2((_input.x > 0) ? 1 : -1, 0);
        }
        else if (_input.y != 0)
        {
            _newTargetDirection = new Vector2(0, (_input.y > 0) ? 1 : -1);
        }
        if (_newTargetDirection != Vector2.zero)
        {
            if (IsMovementCanBeAddToBuffer(_newTargetDirection))
                inputBuffer.Add(_newTargetDirection.normalized);
        }
    }

    bool IsMovementCanBeAddToBuffer(Vector2 _newDirection)
    {
        if (inputBuffer.Count == 0)
            return _newDirection != lastDirection && _newDirection != -lastDirection;
        if (inputBuffer.Count >= MAX_INPUT_BUFFER_SIZE)
            return false;
        if (inputBuffer[inputBuffer.Count - 1] == _newDirection)
            return false;
        return true;
    }
}
