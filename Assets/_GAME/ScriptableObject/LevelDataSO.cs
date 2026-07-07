using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    
    [SerializeField] public List<StageDataSO> stageDatas;

    public List<GateData> gateDatas = new List<GateData>();
}
