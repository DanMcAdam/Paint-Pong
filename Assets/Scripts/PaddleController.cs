using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _pushForce = new Vector2 (0, 10);

    private bool _impulse = false;

    private bool _charging = false;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_impulse)
        {
            _rigidbody.AddForce(_pushForce, ForceMode2D.Impulse);
            _impulse = false;
        }
    }

    public void OnPushPaddle (InputValue input)
    {
        _impulse = true;
    }
}
