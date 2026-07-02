
using System.Collections;
using TMPro;
using UnityEngine;

public class LeaderBoardEntryUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTf;
    [SerializeField] private TextMeshProUGUI rankText;

    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private Vector3 distance;

    [SerializeField] private Vector3 lowestPos;

    [SerializeField] private float timeMove;

    [SerializeField]private int currentRank;

    private int characterId;


    public void OnEnable()
    {
        EventBus<OnRankChange>.Subcribe(ChangeRank);
    }

    public void OnDisable()
    {
        EventBus<OnRankChange>.UnSubcribe(ChangeRank);
    }

    public void SetRankText(int rank)
    {
        currentRank = rank;
        rankText.text = rank.ToString();
        
    }
    public void SetNameText(string name)
    {
        nameText.text= name;
    }

    public void SetInfo(int _rank, string _name, int _characterId)
    {
        SetRankText(_rank);
        SetNameText(_name);
        characterId=_characterId;
    }

    public void ChangeRank(OnRankChange onRankChange)
    {
        
        if(characterId == onRankChange.CharacterId)
        {
           
            SetRankText(onRankChange.NewRank);
            Vector3 target = lowestPos + (4 - onRankChange.NewRank)*distance;
            
            StartCoroutine(IEMovePosition(timeMove, target));
        }
    }

    IEnumerator IEMovePosition(float duration, Vector3 target)
    {
        float timer = 0f;
        Vector3 startPos = rectTf.localPosition;
        while(timer  + 0.01f < duration)
        {
            timer += Time.deltaTime;
            rectTf.localPosition = Vector3.Lerp(startPos, target, timer/duration);
            yield return null;
        }
    }
    

    
}
