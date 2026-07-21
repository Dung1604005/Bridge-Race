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



    public void SetRankUI(List<Character> characters)
    {
     
        for(int i = 0; i < leaderBoard.Count; i++)
        {
            leaderBoard[i].SetInfo(i + 1, characters[i].CharacterName, characters[i].CharacterId);
        }
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
        UIManager.Instance.OpenUI<CanvasSettings>();
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
