using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPixel : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer _sprite;

    private Vector2 _gridPosition;
    public Vector2 GridPosition { get => _gridPosition; set { _gridPosition = value; PrimaryPixel = _gridPosition.x == 0; } }

    private bool _painted = false;

    public bool Painted { get => _painted; set { _painted = value; _sprite.color = _painted? Color.black : Color.white; } }

    public bool PrimaryPixel { get; private set; }

    private PaintGrid _grid;

    public PaintPixel (PaintGrid grid)
    {
        _grid = grid;
    }

}
