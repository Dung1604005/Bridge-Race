using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasVictory : UICanvas
{

    [SerializeField] private List<Image> starList = new List<Image>();

    [SerializeField] private Button nextButton;

    [SerializeField] private TextMeshProUGUI textResult;

    [SerializeField] private TextMeshProUGUI currentGoldText;

    [SerializeField] private TextMeshProUGUI goldAwardText;

    [SerializeField] private int stepAmount;

    [SerializeField] private float stepInterval;


    public override void SetUp()
    {
        base.SetUp();
        gameObject.SetActive(true);
        
        PlayerData playerData = GameData.Instance.PlayerData;
         int goldAward = LevelManager.Instance.RankManager.GetGoldAward();
        SetGoldText(playerData.Gold + goldAward,  currentGoldText);
        StartCoroutine(IEUpdateGoldAnim(0, goldAward, goldAwardText));

        playerData.Gold += goldAward;
        
        GameData.Instance.SavePlayerData(playerData);
        
        int star = LevelManager.Instance.RankManager.CaculateStarPlayer();

        if(star == 0)
        {
            nextButton.gameObject.SetActive(false);
            textResult.text = "LEVEL FAILED!";
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            textResult.text = "LEVEL CLEARED!";
        }
        for(int i = 0; i < starList.Count; i++)
        {
            starList[i].fillAmount = 0f;
        }

        
        for(int i = 0; i < star; i++)
        {
            StartCoroutine(IESlideAnim(1f, starList[i]));
        }

    }
     public void OnHomeButton()
    {
        SoundManager.Instance.PlaySFXClick();
        LevelManager.Instance.DeSpawnLevel();

       
        UIManager.Instance.CloseUI<CanvasVictory>(0f);

        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    public void OnRetry()
    {
        SoundManager.Instance.PlaySFXClick();
        LevelManager.Instance.DeSpawnLevel();
        LevelManager.Instance.InitLevel();
        UIManager.Instance.CloseUI<CanvasVictory>(0f);
        UIManager.Instance.OpenUI<CanvasLoading>(this);
    }

    public void OnNextLevelButton()
    {
        SoundManager.Instance.PlaySFXClick();
        LevelManager.Instance.DeSpawnLevel();
        if(LevelManager.Instance.LevelDataSO.NextLevelData!= null)
        {
            LevelManager.Instance.LoadData(LevelManager.Instance.LevelDataSO.NextLevelData);
            LevelManager.Instance.InitLevel();
            UIManager.Instance.CloseUI<CanvasVictory>(0f);
            UIManager.Instance.OpenUI<CanvasLoading>(this);
        }
        else
        {
            Debug.Log("END GAME");
        }
    }

    public void SetGoldText(int goldAmount, TextMeshProUGUI text)
    {
        // Dua so ve dang string nhu 999,999
        text.text = goldAmount.ToString("N0");
    }


    private IEnumerator IEUpdateGoldAnim(int currentGold, int targetGold, TextMeshProUGUI text)
    {
        int x = 0;
        while(currentGold < targetGold)
        {
            x++;
            if(x > 10000)
            {
                break;
            }
            currentGold += stepAmount;
            SetGoldText(currentGold, text);
            text.text = currentGold.ToString();
            yield return new WaitForSeconds(stepInterval);
        }
    }

    private IEnumerator IESlideAnim(float duration, Image image)
    {
        image.fillAmount = 0f;

        float timer = 0f;
        while(timer + 0.0001f < duration)
        {
            timer += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(0f, 1f, timer/duration);
            yield return null;
        }
    }
}
