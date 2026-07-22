using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BridgeData
{
    public int OwnerStageNumber;

    public int NextStageNumber;

    public TransformData bridgeTFData;

    public TransformData TFPlane;

    public TransformData[] BanisterTFDataArr;

    public StairData[] stairDataArr;
}

[Serializable]

public struct TransformData
{
    public Vector3 Position;

    public Vector3 EulerAngles;

    public Vector3 Scale;
}
[Serializable]
public struct StageData
{
    public int StageNumber;

    public bool IsLastStage;

    public float ScaleX;

    public float ScaleY;

    public float ScaleZ;

    public TransformData[] SpawnPos;
    public TransformData TFData;
}

[Serializable]
public struct GateData
{
    
    public TransformData TFData;

    public int NextStageNumber;


}

[Serializable]
public struct StairData
{
    public TransformData TFData;
}


[Serializable]

public struct LevelData
{
    public int LevelId;

    public int TotalStar;
}

[Serializable]
public struct AllLevelData
{
    public List<LevelData> LevelDatas;
}
[Serializable]

public struct DecorData
{
    public TransformData TFData;

    public int DecorId;
}

[Serializable]

public struct PlayerData
{
    public String PlayerName;
}