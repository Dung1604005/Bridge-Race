using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Scriptable Objects/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    public StageData stageData;

    public List<BridgeData> bridgeDatas = new List<BridgeData>();
}
