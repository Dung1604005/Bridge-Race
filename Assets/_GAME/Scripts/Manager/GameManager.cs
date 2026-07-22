using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager :Singleton<GameManager>
{
    [SerializeField] private PlayerController player;
    [SerializeField]private GameState gameState;

    public GameState GameState => gameState;

    public PlayerController Player => player;

    public void ChangeGameState(GameState newGameState)
    {
        GameState prevState = gameState;
        gameState = newGameState;

        if(gameState == GameState.PLAYING && prevState == GameState.COUNTDOWN)
        {
            OnStart();
        }
        else if(gameState == GameState.PLAYING && prevState == GameState.PAUSED)
        {
            OnContinue();
        }
        else if(gameState == GameState.PAUSED)
        {
            OnPaused();
        }
    }

    public void OnStart()
    {
        LevelManager.Instance.StartGame();   
    }
    public void OnContinue()
    {
        LevelManager.Instance.OnContinue();
    }
    public void OnPaused()
    {
        LevelManager.Instance.OnPause();
    }
    void Awake()
    {
        GameData.Instance.LoadPlayerData();
    }

    void Start()
    {
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }
}


[Serializable]
public enum GameState
{
    MAINMENU = 0,

    PLAYING = 1,

    PAUSED = 2,

    VICTORY = 3,

    DEFEATED = 4,

    
    COUNTDOWN = 6
    
}
