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

public struct LevelDataSave
{
    public int LevelId;

    public int TotalStar;
}

[Serializable]
public struct AllLevelDataSave
{
    public List<LevelDataSave> LevelDatas;
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

    public int Gold;

    public List<int> collectedSkin; 

    public int CurrentSkinId;
}