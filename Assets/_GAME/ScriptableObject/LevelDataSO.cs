using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    public int LevelId;
    public List<StageDataSO> stageDatas;

    public TransformData WinAreaTF;

    public List<GateData> gateDatas = new List<GateData>();

    public List<DecorData> decorObjectDatas = new List<DecorData>();

    public NavMeshData navMeshData;

    public float SpeedBot;

    public float SpeedPlayer;
}
