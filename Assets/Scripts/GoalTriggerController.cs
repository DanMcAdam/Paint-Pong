using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTriggerController : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void GoalScored(BallController ballController)
    {
        ballController.ResetBall();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GoalScored(collision.gameObject.GetComponentInParent<BallController>());
    }
}

enum Player
{
    Player1,
    Player2
}
