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

    private SnakeAudio snakeAudio;

    // Start is called before the first frame update
    void Awake()
    {
        snakeAudio = GetComponent<SnakeAudio>();
    }

    // Reset the snake's position and movement direction, and reset the body snake to its initial state
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

    // Set the reference to the global map data, which is used for checking collisions and apple positions
    public void SetGlobalMapData(GlobalMapData _globalMapData)
    {
        globalMapData = _globalMapData;
    }

    // Set the snake's position and update the body snake's position accordingly
    public void SetPosition(Vector2 _position)
    {
        pos = _position;
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        if (bodySnake != null)
        {
            bodySnake.AddPositionSnake(pos, lastDirection);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver)
            return;
        UpdateMovement();
    }

    // Update the snake's movement based on the timer and input buffer
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
                    EatApple(pos + lastDirection);
                }
                SetPosition(pos + lastDirection);
            } else
            {
                HitWall();
            }
            timer = 0f;
        }
    }

    // Handle the snake hitting a wall or obstacle
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
            bodySnake.ImpactWall();
        }
    }

    // Handle the snake eating an apple
    void EatApple(Vector2 _pos)
    {
        globalMapData.RemoveAppleAtPosition(pos + lastDirection);
        if (bodySnake != null)
        {
            bodySnake.IncreaseLengthSnake();
            bodySnake.SetHeadState(BodySnake.HeadState.Eating);
        }
        if (ComboMananger.Instance != null)
        {
            ComboMananger.Instance.IncrementCombo(pos + lastDirection);
            ScoreManager.instance.AddScore(ComboMananger.Instance.GetComboCount());
        } else
        {
            ScoreManager.instance.AddScore(1);
        }
        if (snakeAudio != null)
        {
            snakeAudio.PlayAppleEatSound();
        }
    }

    // Check if there is an apple at the next position the snake will move to
    bool CheckAppleNextPosition(Vector2 _position)
    {
        if (globalMapData != null)
        {
            return globalMapData.IsAppleTileAtPosition(_position);
        }
        return false;
    }

    // Check if the next position the snake will move to is valid
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

    // Check if the next position the snake will move to collides with its own body
    bool CheckCollisionNextPositionWithBodySnake(Vector2 _position)
    {
        if (bodySnake != null)
        {
            return !bodySnake.IsPositionOnSnake(_position);
        }
        return true;
    }

    // Handle the input for moving the snake, and add valid movement directions to the input buffer
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

    // Check if the new movement direction can be added to the input buffer based on the current direction and buffer state
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
