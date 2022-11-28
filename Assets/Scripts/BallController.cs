using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private float _constantSpeed = 10;

    private Vector2 _startPosition = Vector2.zero;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ResetBall();
    }


    void FixedUpdate()
    {
        Vector2 _slopeFixer = new Vector2(1, .95f);
        _rigidbody2D.velocity = _constantSpeed * (_rigidbody2D.velocity.normalized);
        if (_rigidbody2D.velocity.y / _rigidbody2D.velocity.x <= -1 || _rigidbody2D.velocity.y / _rigidbody2D.velocity.x >= 1)
        {
            _rigidbody2D.velocity = _slopeFixer * (_rigidbody2D.velocity.y);
        }
    }



    public void ResetBall()
    {
        this.gameObject.transform.position = _startPosition;
        _rigidbody2D.AddForce(new Vector2(-20, -15), ForceMode2D.Impulse);
    }
}
