using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    
    public List<StageDataSO> stageDatas;

    public TransformData WinAreaTF;

    public List<GateData> gateDatas = new List<GateData>();

    public List<DecorData> decorObjectDatas = new List<DecorData>();
}
