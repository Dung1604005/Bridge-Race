using System;
using TMPro;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
   [SerializeField] private TextMeshProUGUI goldText;

   [SerializeField] private TMP_InputField nameInputField;

   [SerializeField] private LevelSelectUI levelSelectUI;

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
        UIManager.Instance.OpenUI<LevelSelectUI>();
    }

    public void SetGoldText(int goldAmount)
    {
        // Dua so ve dang string nhu 999,999
        goldText.text = goldAmount.ToString("N0");
    }

    public String GetNameInputField()
    {
        return nameInputField.text;
    }
}
