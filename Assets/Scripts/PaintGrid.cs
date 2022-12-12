using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaintGrid : MonoBehaviour
{
    [SerializeField]
    PaintPixel _pixel;

    [SerializeField]
    Vector3 _localScale = new Vector3(.1f, .1f, 1);

    [SerializeField]
    public Player Player;

    [SerializeField]
    bool _debugAllPaint;

    Dictionary<Vector2, PaintPixel> _paintPixels = new Dictionary<Vector2, PaintPixel>();

    Collider2D _collider;

    ScoreManager _scoreManager;

    PaintGrid _opponentGrid;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        Bounds bounds = _collider.bounds;
        _localScale = new Vector3(bounds.size.x / 3.5f, bounds.size.x / 3f, 1);
        for (int i = 0; i < 300; i++)
        {
            Vector2 gridpos = SetGridPos(i);

            PaintPixel paintPixel = Instantiate(_pixel, this.transform);
            paintPixel.transform.localScale = _localScale;
            paintPixel.transform.position = ConvertToGridPos(ref bounds, gridpos);
            paintPixel.GridPosition = new Vector2(gridpos.x, gridpos.y);
            _paintPixels.Add(paintPixel.GridPosition, paintPixel);
        }
    }

    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
        _scoreManager.RegisterGrid(this);
    }

    private void Update()
    {
        if (_opponentGrid == null)
        {
            _opponentGrid = _scoreManager.FindOpposingGrid(this);
        }
        if (_debugAllPaint)
        {
            foreach (KeyValuePair<Vector2, PaintPixel> entry in _paintPixels)
            {
                entry.Value.Painted = true; 
            }
            _debugAllPaint = false;
        }
    }

    private void OnEnable()
    {
        MenuControl.Restart += Restart;
    }

    private void OnDisable()
    {
        MenuControl.Restart -= Restart;
    }

    private void Restart(string s)
    {
        foreach (KeyValuePair<Vector2, PaintPixel> entry in _paintPixels)
        {
            entry.Value.Painted = false;
        }
    }

    private Vector3 ConvertToGridPos(ref Bounds bounds, Vector2 gridpos)
    {
        return new Vector3(bounds.min.x + _localScale.x / 1.5f + (gridpos.x * (bounds.size.x / 3)), bounds.min.y - _localScale.y / 1.1f + (gridpos.y * (bounds.size.y / 99.5f)), 0);
    }

    private static Vector2 SetGridPos(int i)
    {
        int mathSub = i + 1;
        Vector2 gridpos = new Vector2();
        gridpos.x = Mathf.Floor((i) / 100);
        while (mathSub > 100) { mathSub -= 100; }
        gridpos.y = mathSub;
        return gridpos;
    }

    private PaintPixel FindClosestPixel(Vector2 pos)
    {
        PaintPixel pixel = null;
        float rounded = Mathf.Round((_collider.ClosestPoint(pos).y / (_collider.bounds.size.y / 99.5f)) + (-_collider.bounds.min.y / (_collider.bounds.size.y / 99.5f)) + ((_localScale.y / 1.1f) / (_collider.bounds.size.y / 99.5f)));
        pixel = _paintPixels[new Vector2(0, rounded)];
        return pixel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PaintPixel pixel = FindClosestPixel(collision.transform.position);
        for (int i = 0; i < 3; i++)
        {
            PaintPixel pix = _paintPixels[new Vector2(i, pixel.GridPosition.y)];
            Paint(pix);
            if (_paintPixels.TryGetValue(new Vector2(i, pixel.GridPosition.y + 1), out pix))
                Paint(pix);
            if (_paintPixels.TryGetValue(new Vector2(i, pixel.GridPosition.y - 1), out pix))
                Paint(pix);
        }
    }

    private void Paint(PaintPixel pix)
    {
        if (pix.Painted == true && pix.PrimaryPixel) _opponentGrid.KnockOutRow(pix.GridPosition);
        else if (!pix.Painted && pix.PrimaryPixel)
        {
            pix.Painted = true;
            _scoreManager.UpdateScore(1, Player);
        }
        else pix.Painted = true;
    }

    public void KnockOutRow(Vector2 coords)
    {
        if (_paintPixels.TryGetValue(new Vector2(0, coords.y), out PaintPixel pix))
        {
            if (pix.Painted)
            {
                _scoreManager.UpdateScore(-1, Player);
                pix.Painted = false;
                _paintPixels[new Vector2(1, coords.y)].Painted = false;
                _paintPixels[new Vector2(2, coords.y)].Painted = false;
            }
        }
    }

}
