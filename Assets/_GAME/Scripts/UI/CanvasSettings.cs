using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSettings : UICanvas
{
    [SerializeField] private GameObject toggleSoundOn;

    [SerializeField] private GameObject toggleSoundOff;

    [SerializeField] private Transform homeButton;

    [SerializeField] private Transform retryButton;

    private UICanvas parentCanvas;

    public override void Open(UICanvas uICanvas)
    {
        base.Open(uICanvas);

        parentCanvas = uICanvas;
        if(GameManager.Instance.GameState == GameState.PLAYING)
        {
            GameManager.Instance.ChangeGameState(GameState.PAUSED);
        }
        ChangeValueSound(!GameData.Instance.GetIsMute());

        if(parentCanvas is CanvasMainMenu)
        {
            homeButton.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(false);
        }
        else
        {
            homeButton.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(true);
        }

    }
    public void ChangeValueSound(bool isSoundOn)
    {
        toggleSoundOff.gameObject.SetActive(false);
        toggleSoundOn.gameObject.SetActive(false);

        if (isSoundOn)
        {

            GameData.Instance.SetIsMute(false);
            toggleSoundOn.gameObject.SetActive(true);
        }
        else
        {
           GameData.Instance.SetIsMute(true);
            toggleSoundOn.gameObject.SetActive(false);
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
        
        if(GameManager.Instance.GameState == GameState.PAUSED)
        {
            GameManager.Instance.ChangeGameState(GameState.PLAYING);
        }
        UIManager.Instance.CloseUI<CanvasSettings>(0f);
    }
}
