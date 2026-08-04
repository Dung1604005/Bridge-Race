using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasSkin : UICanvas
{
    [SerializeField] private List<UICharacter> uiCharacters = new List<UICharacter>();

    [SerializeField] private TextMeshProUGUI goldText;

    public override void SetUp()
    {
        base.SetUp();
        GameManager.Instance.GetCharacterModelUI().SetActiveCameraModel(true);
        List<int> collectedSkin = new List<int>();
        List<SkinSO> listSkinSO = GameData.Instance.SkinDatas;
        PlayerData playerData = GameData.Instance.PlayerData;
        SetGoldText(playerData.Gold);

        if(playerData.collectedSkin != null)
        {
            collectedSkin = playerData.collectedSkin;
        }
        for(int i = 0; i < uiCharacters.Count; i++)
        {
            bool collected = false;
            for(int j = 0; j < collectedSkin.Count; j++)
            {
                if(listSkinSO[i].IdSkin == collectedSkin[j])
                {
                    collected = true;
                    break;
                }
            }
            uiCharacters[i].SetUp(listSkinSO[i], collected);
        }

        
    }
    public override void CloseDirectly()
    {
        base.CloseDirectly();
        GameManager.Instance.GetCharacterModelUI().SetActiveCameraModel(false);
    }
    public void OnBackButton()
    {
        SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUTTON_CLICK);
        UIManager.Instance.CloseUIDirectly<CanvasSkin>();

        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    public void SetGoldText(int goldAmount)
    {
        // Dua so ve dang string nhu 999,999
        goldText.text = goldAmount.ToString("N0");
    }

    public void TurnOffAllFocusEffect()
    {
        foreach(UICharacter uICharacter in uiCharacters)
        {
            uICharacter.SetActiveFocusEffect(false);
        }
    }






}
