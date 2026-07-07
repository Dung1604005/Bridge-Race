using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Scriptable Objects/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    public StageData StageData;

    public List<BridgeData> BridgeDatas = new List<BridgeData>();

}
