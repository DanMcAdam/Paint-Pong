using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleAIController : PaddleController
{
    [SerializeField]
    [Range(0, 100)]
    int _chanceForRandom;

    [SerializeField]
    [Range(0, 30)]
    float _timeBetweenRandomCheck;

    [SerializeField]
    [Range(1, 30)]
    float _timeForRandomMin;

    [SerializeField]
    [Range(1, 30)]
    float _timeForRandomMax;

    float _timeSinceLastRandom;
    float _timeForRandom;
    float _randomMoveDir;
    ScoreManager _scoreManager;

    AiBall _ball;

    Vector2 _ballPos;
    Vector2 _lastBallPos;



    bool _randomMove = false;
    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _startPos = transform.position;
        _scoreManager = ScoreManager.Instance;
    }

    private void Update()
    {
        HandleRandomMovementCode();
        if (_ball == null)
        {
            _ball = _scoreManager.AIBall;
            _ballPos = _ball.transform.position;
        }
    }

    private void HandleRandomMovementCode()
    {
        _timeSinceLastRandom += Time.deltaTime;
        if (_timeSinceLastRandom >= _timeBetweenRandomCheck && !_randomMove)
        {
            if (Random.Range(0, 100) < _chanceForRandom)
            {
                _randomMove = true;
                _randomMoveDir = Random.Range(-1, 1);
                _timeForRandom = Random.Range(_timeForRandomMin, _timeForRandomMax);
                _timeSinceLastRandom = 0;
            }
        }
        if (_randomMove)
        {

            if (_timeSinceLastRandom > _timeForRandom)
            {
                _randomMove = false;
                _timeSinceLastRandom = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_ball != null)
        {
            _lastBallPos = _ballPos;
            _ballPos = _ball.transform.position;
            if (_ballPos == _lastBallPos)
            {
                _ball.GetComponent<BallController>().ResetBall();
            }
            HandleMovement();
            HandleRotation();
        }
    }

    private void LateUpdate()
    {

    }

    internal void HandleRotation()
    {
        if (!_randomMove && Mathf.Approximately(_ballPos.y, _lastBallPos.y) && Vector2.Distance(_ballPos, transform.position) < 2)
        {
            _rb2d.rotation = -Mathf.Sign(transform.position.y) * _rotationClamp.y;
        }
        else
        {
            _rb2d.rotation = 0f;
            _rb2d.freezeRotation = true;
        }
    }

    internal void HandleMovement()
    {
        if (_ballPos.x > _lastBallPos.x)
        {
            if (!_randomMove && Mathf.Abs(_ballPos.y - transform.position.y) > .5f)
            {

                float moveDir = _ball.Pos.y > transform.position.y ? 1f : -1f;
                _rb2d.MovePosition(transform.position + new Vector3(0, (moveDir) * Time.deltaTime * _moveSpeed));

            }
            else if (_randomMove)
            {
                Debug.Log("acting randomly");
                _rb2d.MovePosition(transform.position + new Vector3(0, (_randomMoveDir) * Time.deltaTime * _moveSpeed));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        return;
    }
}
