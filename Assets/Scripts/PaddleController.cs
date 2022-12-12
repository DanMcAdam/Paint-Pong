using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    [SerializeField]
    internal float _moveSpeed = 10;
    [SerializeField]
    internal float _rotationSpeed = 200;
    [SerializeField]
    internal Vector2 _rotationClamp;

    public DefaultInputActions MoveAction { get; set; }

    private Vector2 _playerInput;

    internal Rigidbody2D _rb2d;
    internal Vector2 _startPos;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        MoveAction = new DefaultInputActions();
    }

    void Start()
    {
        _startPos = transform.position;
        MoveAction.Player.Move.Enable();
    }


    void Update()
    {
        _playerInput = MoveAction.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        HandleMovement();

        HandleRotation();

    }
    private void HandleMovement()
    {
        _rb2d.MovePosition(transform.position + new Vector3(0, _playerInput.y, 0) * Time.deltaTime * _moveSpeed);
    }

    internal void HandleRotation()
    {
        if ((_playerInput.x <= .1 && _playerInput.x >= -.1) && _rb2d.rotation != 0)
        {
            _rb2d.rotation = 0f;
            //float rotationDir = _rb2d.rotation > 0 ? -1 : 1;
            //_rb2d.MoveRotation(_rb2d.rotation + (rotationDir * _rotationSpeed) * Time.fixedDeltaTime);
        }
        else if (_rb2d.rotation < _rotationClamp.y && _rb2d.rotation > _rotationClamp.x)
        {
            _rb2d.MoveRotation(_rb2d.rotation + (_playerInput.x * _rotationSpeed) * Time.fixedDeltaTime);
        }
        else if (_rb2d.rotation > _rotationClamp.y && _playerInput.x < 0)
        {
            _rb2d.MoveRotation(_rb2d.rotation + (_playerInput.x * _rotationSpeed) * Time.fixedDeltaTime);
        }
        else if (_rb2d.rotation < _rotationClamp.x && _playerInput.x > 0)
        {
            _rb2d.MoveRotation(_rb2d.rotation + (_playerInput.x * _rotationSpeed) * Time.fixedDeltaTime);
        }

        if (_rb2d.rotation < _rotationClamp.x)
        {
            _rb2d.rotation = _rotationClamp.x;
        }
        if (_rb2d.rotation > _rotationClamp.y)
        {
            _rb2d.rotation = _rotationClamp.y;
        }
    }

    internal void OnEnable()
    {
        MenuControl.Restart += Restart;
    }

    internal void OnDisable()
    {
        MenuControl.Restart -= Restart;
    }

    internal void Restart(string s)
    {
        transform.position = _startPos;
    }

    internal void OnCollisionStay2D(Collision2D collision)
    {
        _rb2d.freezeRotation = true;
    }

    internal void OnCollisionExit2D(Collision2D collision)
    {
        _rb2d.freezeRotation = false;
    }
}
