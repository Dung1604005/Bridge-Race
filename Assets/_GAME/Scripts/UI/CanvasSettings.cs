using System;
using UnityEngine;

public class CanvasSettings : UICanvas
{
    [SerializeField] private GameObject toggleSoundOn;

    [SerializeField] private GameObject toggleSoundOff;

    public override void Open()
    {
        base.Open();
        GameManager.Instance.ChangeGameState(GameState.PAUSED);
    }
    public void ChangeValueSound(bool isSoundOn)
    {
        toggleSoundOff.gameObject.SetActive(false);
        toggleSoundOn.gameObject.SetActive(false);

        if (isSoundOn)
        {
            PlayerData playerData = GameData.Instance.PlayerData;

            playerData.IsMute = false;

            GameData.Instance.SavePlayerData(playerData);
            toggleSoundOn.gameObject.SetActive(true);
        }
        else
        {
            PlayerData playerData = GameData.Instance.PlayerData;

            playerData.IsMute = true;

            GameData.Instance.SavePlayerData(playerData);
            toggleSoundOff.gameObject.SetActive(true);
        }
    }

    public void OnHomeButton()
    {
        
        LevelManager.Instance.DeSpawnLevel();

        UIManager.Instance.CloseUI<CanvasSettings>(0f);
        UIManager.Instance.CloseUI<CanvasGamePlay>(0f);

        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    public void OnRetry()
    {
        
        LevelManager.Instance.DeSpawnLevel();
        LevelManager.Instance.InitLevel();
        UIManager.Instance.CloseUI<CanvasSettings>(0f);
        UIManager.Instance.CloseUI<CanvasGamePlay>(0f);
        UIManager.Instance.OpenUI<CanvasLoading>(this);
        
        
    }
    public void OnContinue()
    {
       
        GameManager.Instance.ChangeGameState(GameState.PLAYING);
        UIManager.Instance.CloseUI<CanvasSettings>(0f);
    }
}
