using System;
using UnityEngine;

public class GameManager :Singleton<GameManager>
{
    [SerializeField]private GameState gameState;

    public GameState GameState => gameState;

    public void ChangeGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    public void StartGame()
    {
        ChangeGameState(GameState.PLAYING);
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

    LOADING = 5
    
}
