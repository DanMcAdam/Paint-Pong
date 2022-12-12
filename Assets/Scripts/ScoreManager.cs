using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public MenuControl MenuControl { get; private set; }

    public float MatchLength { get; private set; }

    public int PlayerOneScore { get; private set; }

    public int PlayerTwoScore { get; private set; }

    public AiBall AIBall { get; private set; }

    public PaintGrid PlayerOneGrid { get; private set; }
    public PaintGrid PlayerTwoGrid { get; private set; }

    public bool MatchActive { get; set; } = false;

    private TextMeshProUGUI _playerOneScoreDisplay;
    private TextMeshProUGUI _playerTwoScoreDisplay;
   
    private string[] _scoreArray = new string[101];

    
    private void Awake()
    {
        MatchLength = 30f;

        for (int i = 0; i < _scoreArray.Length; i++)
        {
            _scoreArray[i] = i.ToString();
        }

        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public PaintGrid FindOpposingGrid (PaintGrid paintGrid)
    {
        if (paintGrid == PlayerOneGrid)
        {
            return PlayerTwoGrid;
        }
        else return PlayerOneGrid; 
    }

    public void RegisterGrid (PaintGrid paintGrid)
    {
        if (paintGrid.Player == Player.Player1)
        {
            PlayerOneGrid = paintGrid;
        }
        else PlayerTwoGrid = paintGrid;
    }

    public void RegisterMenuControl (MenuControl control)
    {
        MenuControl = control;
    }

    public void RegisterScoreDisplay (TextMeshProUGUI display, Player player)
    {
        if (player == Player.Player1)
        {
            _playerOneScoreDisplay = display;
        }
        else _playerTwoScoreDisplay = display;
    }

    internal void RegisterAIBall(AiBall aiBall)
    {
        AIBall = aiBall;
    }

    public void UpdateScore(int score, Player player)
    {
        if (Player.Player1 == player)
        {
            PlayerOneScore += score;
            _playerOneScoreDisplay.text = _scoreArray[PlayerOneScore];
        }
        else
        {
            PlayerTwoScore += score;
            _playerTwoScoreDisplay.text = _scoreArray[PlayerTwoScore];
        }
    }

    public void EndMatch()
    {
        string winner = " has won!";
        Color winnerColor = Color.white;
        if (PlayerOneScore > PlayerTwoScore)
        {
            winner = "Player One" + winner;
            winnerColor = _playerOneScoreDisplay.color;
        }
        else if (PlayerTwoScore > PlayerOneScore)
        {
            winner = "Player Two" + winner;
            winnerColor = _playerTwoScoreDisplay.color;
        }
        else winner = "Tie!";

        MatchActive = false;

        MenuControl.GameOver(winner, winnerColor);
    }

    private void Restart(string s)
    {
        PlayerOneScore = 0;
        PlayerTwoScore = 0;
        _playerOneScoreDisplay.text = _scoreArray[PlayerOneScore];
        _playerTwoScoreDisplay.text = _scoreArray[PlayerTwoScore];
        
        MatchActive = true;
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
