using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private List<Stage> stages;



    public Stage GetStage(int stageNumber)
    {
        for(int i = 0; i < stages.Count; i++)
        {
            if(stages[i].StageNumber == stageNumber)
            {
                return stages[i];
            }
        }
        Debug.LogError("STAGE " + stageNumber + " DONT EXIST IN STAGE MANAGER");
        return null;
    }
}
