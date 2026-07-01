using System.Collections.Generic;
using UnityEngine;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private List<LeaderBoardEntryUI> leaderBoard = new List<LeaderBoardEntryUI>();



    public void SetRankUI(List<Character> characters)
    {
        Debug.Log(leaderBoard.Count);
        for(int i = 0; i < leaderBoard.Count; i++)
        {
            leaderBoard[i].SetInfo(i + 1, characters[i].CharacterName, characters[i].CharacterId);
        }
    }
}
