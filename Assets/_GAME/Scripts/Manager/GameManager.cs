using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager :Singleton<GameManager>
{
    [SerializeField] private PlayerController player;
    [SerializeField]private GameState gameState;

    [SerializeField] private CharacterModelUI characterModelUI;

    public GameState GameState => gameState;

    public PlayerController GetPlayer() {return player;}

    public CharacterModelUI GetCharacterModelUI() {return characterModelUI;}

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
        else if(gameState == GameState.VICTORY)
        {
            Invoke(nameof(OnVictory), 5f);
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
    public void OnVictory()
    {
        UIManager.Instance.CloseAllDirectly();
        UIManager.Instance.OpenUI<CanvasVictory>();
    }

    public void OnInit()
    {
         QualitySettings.vSyncCount = 0;

    
        Application.targetFrameRate = 60;
        GameData.Instance.LoadPlayerData();
        GameData.Instance.LoadLevelData();
    }
    void Awake()
    {
       OnInit();
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
