using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class Bridge : GameUnit
{
    [SerializeField] private StageDataSO dataSO;
    [SerializeField] private List<Stair> stairs = new List<Stair>();

    [SerializeField] private Stage ownerStage;

    [SerializeField] private Stage nextStage;

    //Luu tranform lan can cua bridge :D
    [SerializeField] private List<Transform> banisters = new List<Transform>();

    public Stage NextStage => nextStage;

    public Stage OwnerStage => ownerStage;

    public List<Stair> Stairs => stairs;

    public void LoadData(BridgeData bridgeData)
    {
        Helper.LoadTransformData(tf, bridgeData.bridgeTFData);
        if(banisters.Count != bridgeData.BanisterTFDataArr.Length)
        {
            Debug.LogError("BRIDGE DATA DONT HAVE ENOUGH BANISTER DATA");
        }

        for(int i = 0; i < banisters.Count; i++)
        {
            Helper.LoadTransformData(banisters[i], bridgeData.BanisterTFDataArr[i]);
        }
        SetOwnerStage(StageManager.Instance.GetStage(bridgeData.OwnerStageNumber));

        SetNextStage(StageManager.Instance.GetStage(bridgeData.NextStageNumber));
    }

    public void SetOwnerStage(Stage _ownerStage)
    {
        ownerStage = _ownerStage;
    }

    public void SetNextStage(Stage _nextStage)
    {
        nextStage = _nextStage;
    }

    public int GetAmountColorStair(ColorType colorType)
    {
        int ans = 0;
        for(int i = 0; i < stairs.Count; i++)
        {
            if(stairs[i].ColorType == colorType)
            {
                ans += 1;
            }
        }
        return ans;
    }

    public int GetStairId(Stair stair)
    {
        for(int i = 0; i < stairs.Count; i++)
        {
            if(stairs[i] == stair)
            {
                return i;
            }
        }
        return -1;
    }

    public StairInfo GetFarthestStairPossible(int currentStair, ColorType colorType, int number)
    {
        
        int farthestStair = currentStair;
        for(int i = currentStair + 1; i < stairs.Count; i++)
        {
            if(stairs[i].ColorType != colorType)
            {
                if(number == 0)
                {
                    return new StairInfo
                    {
                        stairId = farthestStair,
                        isLastStair = (farthestStair == stairs.Count - 1) ? true:false,
                        position = stairs[farthestStair].transform.position
                    };
                }
                number -= 1;
            }
            farthestStair = i;

        }
        return new StairInfo
        {
           stairId = farthestStair,
           isLastStair = (farthestStair == stairs.Count - 1) ? true:false,
           position = stairs[farthestStair].transform.position  
        };
    }

    [ContextMenu("CREATE DATA")]

    public void ExtractDataToSO()
    {
        BridgeData data = new BridgeData();

        data.OwnerStageNumber = ownerStage.StageNumber;
        data.NextStageNumber = nextStage.StageNumber;

        data.bridgeTFData = Helper.CreateDataFromTransform(tf);
        TransformData[] banisterTFDataArr = new TransformData[2];

        banisterTFDataArr[0] = Helper.CreateDataFromTransform(banisters[0].transform);
        banisterTFDataArr[1] = Helper.CreateDataFromTransform(banisters[1].transform);

        StairData[] stairDataArr = new StairData[stairs.Count];

        for(int i = 0; i < stairs.Count; i++)
        {
            stairDataArr[i].TFData = Helper.CreateDataFromTransform(stairs[i].TF);
        }

        dataSO.bridgeDatas.Add(data);


        EditorUtility.SetDirty(dataSO);
        AssetDatabase.SaveAssets();

    }

    void Start()
    {
        foreach(Stair stair in stairs)
        {
            stair.SetBridge(this);
        }
    }

}


public struct StairInfo
{
    public int stairId;

    public bool isLastStair;
    public Vector3 position;
}
[Serializable]
public struct BridgeData
{
    public int OwnerStageNumber;

    public int NextStageNumber;

    public TransformData bridgeTFData;

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

