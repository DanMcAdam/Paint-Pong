using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private float _baseSpeed = 10;
    [SerializeField]
    private float _maxSpeed = 15;
    [SerializeField]
    private float _speedIncreaseRate = .01f;


    [SerializeField]
    Player _player;

    private float _currentSpeed;

    private Vector2 _startPosition;

    private Rigidbody2D _rigidbody2D;

    private AudioSource _audioSource;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponentInChildren<AudioSource>();
        _startPosition = _player == Player.Player1 ? new Vector2(-3, 0) : new Vector2(3, 0);
    }

    void Start()
    {
        ResetBall();
    }


    void FixedUpdate()
    {
        Vector2 _slopeFixer = new Vector2(1, .95f);
        _rigidbody2D.velocity = _currentSpeed * (_rigidbody2D.velocity.normalized);
        if (_rigidbody2D.velocity.y / _rigidbody2D.velocity.x <= -1 || _rigidbody2D.velocity.y / _rigidbody2D.velocity.x >= 1)
        {
            _rigidbody2D.velocity = _slopeFixer * (_rigidbody2D.velocity.y);
        }
        _currentSpeed = _currentSpeed < _maxSpeed? _currentSpeed += _speedIncreaseRate : _currentSpeed ;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _audioSource.Play();
    }



    public void ResetBall()
    {
        _currentSpeed = _baseSpeed;
        float startDir = _player == Player.Player1 ? -1 : 1;
        Vector2 randomdir = new Vector2(startDir, Random.Range(-10f, 10f));
        this.gameObject.transform.position = _startPosition;
        _rigidbody2D.AddForce(randomdir, ForceMode2D.Impulse);
    }

    private void Restart(string s)
    {
        ResetBall();
    }

    private void OnEnable()
    {
        MenuControl.Restart += Restart;
    }

    private void OnDisable()
    {
        MenuControl.Restart -= Restart;
    }
}
