using System;
using TMPro;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI goldText;

    [SerializeField] private TMP_InputField nameInputField;


    public override void SetUp()
    {
        base.SetUp();
        GameManager.Instance.ChangeGameState(GameState.MAINMENU);
        GameManager.Instance.GetCharacterModelUI().SetActiveCameraModel(true);
        EventBus<OnLoadSkinModel>.Raise(new OnLoadSkinModel
        {
            SkinId = GameData.Instance.GetCurrentSkin()
        });
        SetNameInputField(GameManager.Instance.GetPlayer().CharacterName);
        SetGoldText(GameData.Instance.GetGold());
    }

    public override void Close(float time)
    {
        base.Close(time);
        GameManager.Instance.GetCharacterModelUI().SetActiveCameraModel(false);
    }
    public void SettingButton()
    {
        SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUTTON_CLICK);
        UIManager.Instance.OpenUI<CanvasSettings>(this);
    }

    public void SkinButton()
    {
        SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUTTON_CLICK);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasSkin>();
    }

    public void ShopButton()
    {
        SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUTTON_CLICK);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasSkin>();
    }

    public void SelectLevelButton()
    {
        SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUTTON_CLICK);
        UIManager.Instance.OpenUI<CanvasLevelSelect>();
    }

    public void SetGoldText(int goldAmount)
    {
        goldText.text = goldAmount.ToString("N0");
    }

    public void OnNameInputFieldChange(String namePlayer)
    {
        GameManager.Instance.GetPlayer().SetName(namePlayer);
        
        GameData.Instance.SaveNamePlayerData(namePlayer);
    }

    public void SetNameInputField(String namePlayer)
    {
        nameInputField.text = namePlayer;
    }
}
