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
            toggleSoundOn.gameObject.SetActive(true);
        }
        else
        {
            toggleSoundOff.gameObject.SetActive(true);
        }
    }

    public void OnHomeButton()
    {
        SoundManager.Instance.PlaySFXClick();
        LevelManager.Instance.DeSpawnLevel();

        UIManager.Instance.CloseUI<CanvasSettings>(0f);
        UIManager.Instance.CloseUI<CanvasGamePlay>(0f);

        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    public void OnRetry()
    {
        SoundManager.Instance.PlaySFXClick();
        LevelManager.Instance.DeSpawnLevel();
        LevelManager.Instance.InitLevel();
        UIManager.Instance.CloseUI<CanvasSettings>(0f);
        UIManager.Instance.CloseUI<CanvasGamePlay>(0f);
        UIManager.Instance.OpenUI<CanvasLoading>(this);
        
        
    }
    public void OnContinue()
    {
        SoundManager.Instance.PlaySFXClick();
        GameManager.Instance.ChangeGameState(GameState.PLAYING);
        UIManager.Instance.CloseUI<CanvasSettings>(0f);
    }
}
