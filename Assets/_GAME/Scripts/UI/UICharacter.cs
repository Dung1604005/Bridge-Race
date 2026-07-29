using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour
{
    [SerializeField] private SkinSO skinSO;

    [SerializeField] private TextMeshProUGUI nameSkin;

    [SerializeField] private Image portrait;

    [SerializeField] private UnityEngine.UI.Button selectButton;

    [SerializeField] private UnityEngine.UI.Button buyButton;

    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private GameObject effectFocus;

    public void SetUp(SkinSO _skinSO, bool collected)
    {
        this.skinSO = _skinSO;

        portrait.sprite = _skinSO.SkinPortrait;

        nameSkin.text = _skinSO.NameSkin;

        SetActiveSkin(collected);
    }

    public void SetActiveSkin(bool collected)
    {
         buyButton.gameObject.SetActive(false);

        selectButton.gameObject.SetActive(false);

        effectFocus.gameObject.SetActive(false);

        if (collected)
        {
           
            selectButton.gameObject.SetActive(true);
        }
        else
        {
            priceText.text =  skinSO.Price.ToString("N0");
            buyButton.gameObject.SetActive(true);
        }
    }

    public void OnBuyButton()
    {
        PlayerData playerData = GameData.Instance.PlayerData;

        if(playerData.Gold < skinSO.Price)
        {
            CanvasNotification canvas = UIManager.Instance.OpenUI<CanvasNotification>();
            canvas.SetText("Error", "You don't have enough gold to buy this skin");
        }
        else
        {
            
            playerData.collectedSkin.Add(skinSO.IdSkin);
            playerData.Gold -= skinSO.Price;
            GameData.Instance.SavePlayerData(playerData);
            CanvasSkin canvasSkin = UIManager.Instance.GetUI<CanvasSkin>();
            canvasSkin.SetGoldText(playerData.Gold);

            SetActiveSkin(true);
        }
    }

    public void OnSelectButton()
    {
        EventBus<OnLoadSkinModel>.Raise(new OnLoadSkinModel
        {
            SkinId = skinSO.IdSkin
        });
        effectFocus.SetActive(true);
        PlayerData playerData = GameData.Instance.PlayerData;

        playerData.CurrentSkinId = skinSO.IdSkin;

        GameData.Instance.SavePlayerData(playerData);

        GameManager.Instance.Player.SetSkin(skinSO.SkinPrefab);

        


    }
}
