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
        GameManager.Instance.GetCharacterModelUI().SetActiveCameraModel(true);
        EventBus<OnLoadSkinModel>.Raise(new OnLoadSkinModel
        {
            SkinId = GameData.Instance.PlayerData.CurrentSkinId
        });
        SetNameInputField(GameManager.Instance.GetPlayer().CharacterName);
        SetGoldText(GameData.Instance.PlayerData.Gold);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        GameManager.Instance.GetCharacterModelUI().SetActiveCameraModel(false);
    }
   public void SettingButton()
    {
        SoundManager.Instance.PlaySFXClick();
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasSettings>();
    }

    public void SkinButton()
    {
        SoundManager.Instance.PlaySFXClick();
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasSkin>();
    }

    public void ShopButton()
    {
        SoundManager.Instance.PlaySFXClick();
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasSkin>();
    }

    public void SelectLevelButton()
    {
        SoundManager.Instance.PlaySFXClick();
        UIManager.Instance.OpenUI<CanvasLevelSelect>();
    }

    public void SetGoldText(int goldAmount)
    {
        // Dua so ve dang string nhu 999,999
        goldText.text = goldAmount.ToString("N0");
    }

    public void OnNameInputFieldChange(String namePlayer)
    {
        GameManager.Instance.GetPlayer().SetName(namePlayer);
        PlayerData playerData = GameData.Instance.PlayerData;
        playerData.PlayerName = namePlayer;
        GameData.Instance.SavePlayerData(playerData);
    }

    public void SetNameInputField(String namePlayer)
    {
        nameInputField.text = namePlayer;
    }
}
