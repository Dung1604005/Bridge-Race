
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    [SerializeField] private int levelId;

    [SerializeField] private List<StageDataSO> stageDataSOs = new List<StageDataSO>();

    [SerializeField] private TransformData winAreaTF;

    [SerializeField] private List<GateData> gateDatas = new List<GateData>();

    [SerializeField] private List<DecorData> decorObjectDatas = new List<DecorData>();

    [SerializeField] private NavMeshData navMeshData;

    [SerializeField] private float speedBot;

    [SerializeField] private float speedPlayer;

    [SerializeField] private int goldPerStar;

    [SerializeField] private LevelDataSO nextLevelData;
    public int LevelId => levelId;
    public List<StageDataSO> StageDataSOs => stageDataSOs;

    public TransformData WinAreaTF => winAreaTF;

    public List<GateData> GateDatas => gateDatas;

    public List<DecorData> DecorObjectDatas => decorObjectDatas;

    public NavMeshData NavMeshData => navMeshData;

    public float SpeedBot => speedBot;

    public float SpeedPlayer => speedPlayer;

    public int GoldPerStar => goldPerStar;

    public LevelDataSO NextLevelData => nextLevelData;

    public void SetDecorObjectDatas(List<DecorData> decorDatas)
    {
        decorObjectDatas = decorDatas;
    }

    public void SetWinAreaTF(TransformData winDataTF)
    {
        winAreaTF = winDataTF;
    }

    public void AddGateData(GateData gateData)
    {
        gateDatas.Add(gateData);
    }
}
