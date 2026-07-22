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
        SetNameInputField(GameManager.Instance.Player.CharacterName);
    }
   public void SettingButton()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasSettings>();
    }

    public void SkinButton()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasSkin>();
    }

    public void ShopButton()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasSkin>();
    }

    public void SelectLevelButton()
    {
        UIManager.Instance.OpenUI<CanvasLevelSelect>();
    }

    public void SetGoldText(int goldAmount)
    {
        // Dua so ve dang string nhu 999,999
        goldText.text = goldAmount.ToString("N0");
    }

    public void OnNameInputFieldChange(String namePlayer)
    {
        GameManager.Instance.Player.SetName(namePlayer);
    }

    public void SetNameInputField(String namePlayer)
    {
        nameInputField.text = namePlayer;
    }
}
