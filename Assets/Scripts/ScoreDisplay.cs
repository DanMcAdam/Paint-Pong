using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private TextMeshProUGUI _text;

    private bool _registered = false;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_text != null && !_registered)
        {
            ScoreManager.Instance.RegisterScoreDisplay(_text, _player);
            _registered = true;
        }
    }
}
