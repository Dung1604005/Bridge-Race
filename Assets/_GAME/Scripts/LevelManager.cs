using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
   [SerializeField] private RankManager rankManager;

   [SerializeField] private WinArea winArea;

   public RankManager RankManager => rankManager;
    public Vector3 GetWinAreaPosition()
    {
        return winArea.TF.position;
    }

    void Start()
    {
        List<Character> characters = rankManager.GetRankedList();
        UIManager.Instance.GetUI<CanvasGamePlay>().SetRankUI(characters);

    }
  
}

