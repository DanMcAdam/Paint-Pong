using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuControl : MonoBehaviour
{
    [SerializeField]
    GameObject _mainMenuScreen;
    [SerializeField]
    Button _startButton;
    [SerializeField]
    Button _creditsButton;

    [SerializeField]
    GameObject _creditsScreen;
    [SerializeField]
    Button _backButton;

    [SerializeField]
    GameObject _tutorialScreen;

    [SerializeField]
    GameObject _pauseScreen;
    [SerializeField]
    Button _resumeButton;
    [SerializeField]
    Button _restartButton;
    [SerializeField]
    Button _mainMenuButton;
    [SerializeField]
    Slider _soundSlider;


    [SerializeField]
    GameObject _gameOverScreen;
    [SerializeField]
    TextMeshProUGUI _gameOverText;
    [SerializeField]
    Button _gameOverRestartButton;
    [SerializeField]
    Button _gameOverMainMenuButton;

    public static Action<string> Restart;

    bool _isPaused;
    bool _inGame;

    GameObject[] _overlays;
    private void Start()
    {
        ScoreManager.Instance.RegisterMenuControl(this);
        _overlays = new GameObject[] { _gameOverScreen, _pauseScreen, _mainMenuScreen };
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscPressed();
        }

    }

    public void GameOver(String winnerMessage, Color winnerColor)
    {
        Time.timeScale = 0;
        _gameOverScreen.SetActive(true);
        _gameOverText.color = winnerColor;
        _gameOverText.text = winnerMessage;
    }

    public void OnEscPressed()
    {
        if (ScoreManager.Instance.MatchActive == true)
        {
            if (_isPaused)
            {
                OnResumePressed();
            }
            else
            {
                _isPaused = true;
                Time.timeScale = 0;
                _pauseScreen.SetActive(true);
            }
        }
        else if (_creditsScreen.activeInHierarchy)
        {
            _creditsScreen.SetActive(false);
        }
        else if (_tutorialScreen.activeInHierarchy)
        {
            _tutorialScreen.SetActive(false);
        }
    }

    public void OnResumePressed()
    {
        _isPaused = false;
        _pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnRestartPressed()
    {
        Debug.Log("Restart Pressed");
        foreach (GameObject overlay in _overlays)
        {
            overlay.SetActive(false);
        }
        Restart?.Invoke("");

        //TODO Add restart "press button to start" 
        Time.timeScale = 1;
    }

    public void OnMainMenuPressed()
    {
        //Decided to use unity events to handle menu transitions for Main menu, this handles gamestate logic
        ScoreManager.Instance.MatchActive = false;
    }

    public void SoundControl(float newValue)
    {
        AudioListener.volume = newValue;
    }
}
