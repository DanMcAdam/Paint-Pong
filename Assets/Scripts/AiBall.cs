using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBall : MonoBehaviour
{
    public Vector2 Pos { get; private set; }

    ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
        _scoreManager.RegisterAIBall(this);
    }

    private void FixedUpdate()
    {
        Pos = transform.position;
    }
}
