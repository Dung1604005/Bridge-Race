using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Scriptable Objects/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    [SerializeField] private StageData stageData;

    public StageData StageData => stageData;

    public List<BridgeData> BridgeDatas = new List<BridgeData>();

    public void SetStageData(StageData stageData)
    {
        this.stageData = stageData;
    }

}
