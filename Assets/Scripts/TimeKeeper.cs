using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeKeeper : MonoBehaviour
{
    [SerializeField]
    Slider _slider;

    public float MatchLength;

    private float _startTime;
    private float _timeElapsed;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    void Start()
    {
        MatchLength = ScoreManager.Instance.MatchLength;
        _slider.maxValue = MatchLength;
        _slider.value = MatchLength;
    }


    void Update()
    {
        _timeElapsed += Time.deltaTime;
        float time = MatchLength - _timeElapsed;
        _slider.value = time;
        if (time <= 0) ScoreManager.Instance.EndMatch();
    }

    private void Restart(string s)
    {
        _timeElapsed = 0;
        _slider.value = MatchLength;
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
