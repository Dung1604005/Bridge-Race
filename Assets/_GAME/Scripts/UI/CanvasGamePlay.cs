using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private Transform panelCountDownRoot;
    [SerializeField] private int countDownTime;

    [SerializeField] private TextMeshProUGUI countDownText;

    [SerializeField] private Vector3 startScaleCountDownText;

    [SerializeField] private Vector3 targetScaleCountDownText;
    
    [SerializeField] private float durationEffect;

    [SerializeField] private List<LeaderBoardEntryUI> leaderBoard = new List<LeaderBoardEntryUI>();

    [SerializeField] private TextMeshProUGUI textLevel;

    private bool isSetUp = false;



    public void SetRankUI(List<Character> characters)
    {
     
        for(int i = 0; i < leaderBoard.Count; i++)
        {
        
            if (!isSetUp)
            {
                leaderBoard[i].SetInfo(i + 1, characters[i].CharacterName, characters[i].CharacterId);
            }
            EventBus<OnRankChange>.Raise(new OnRankChange
            {
                CharacterId = characters[i].CharacterId,
                NewRank = i + 1
            });
            
        }
    }

    public void SetLevelText(int level)
    {
        textLevel.text = "Level "+level.ToString();
    }

    public override void SetUp()
    {
        base.SetUp();
        gameObject.SetActive(true);
        if (!isSetUp)
        {
            SetRankUI(LevelManager.Instance.RankManager.GetRankedList());
            isSetUp = true;
        }
        
        SetLevelText(LevelManager.Instance.LevelDataSO.LevelId + 1);
    }

    public void StartCountDown()
    {
        GameManager.Instance.ChangeGameState(GameState.COUNTDOWN);
        panelCountDownRoot.gameObject.SetActive(true);
        
        StartCoroutine(IECountDown());
    }

    public void AnimateTextCountDown()
    {
        countDownText.transform.localScale = startScaleCountDownText;

        StartCoroutine(Helper.IEDoScaleOutBack(countDownText.transform, targetScaleCountDownText, durationEffect, 0.2f));
    }

    public void OnSettingButton()
    {
        SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUTTON_CLICK);
        UIManager.Instance.OpenUI<CanvasSettings>(this);
    }

    private IEnumerator IECountDown()
    {
        int timer = 0;
        while(timer < countDownTime)
        {
            countDownText.text = (3 - timer).ToString();
            AnimateTextCountDown();
            timer += 1;
            yield return new WaitForSeconds(1);
        }
        countDownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        panelCountDownRoot.gameObject.SetActive(false);
        GameManager.Instance.ChangeGameState(GameState.PLAYING);
    }
}
