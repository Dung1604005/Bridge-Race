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

    [SerializeField] private Transform plane;

    //Luu tranform lan can cua bridge :D
    [SerializeField] private List<Transform> banisters = new List<Transform>();

    public Stage NextStage => nextStage;

    public Stage OwnerStage => ownerStage;

    public List<Stair> Stairs => stairs;

    [ContextMenu("CREATE DATA")]

    public void ExtractDataToSO()
    {
        BridgeData data = new BridgeData();

        data.OwnerStageNumber = ownerStage.StageNumber;

        if(nextStage == null)
        {
            data.NextStageNumber = -1;
        }
        else
        {
            data.NextStageNumber = nextStage.StageNumber;
        }

        data.TFPlane = Helper.CreateDataFromTransform(plane);

        data.bridgeTFData = Helper.CreateDataFromTransform(tf);
        TransformData[] banisterTFDataArr = new TransformData[2];

        banisterTFDataArr[0] = Helper.CreateDataFromTransform(banisters[0].transform);
        banisterTFDataArr[1] = Helper.CreateDataFromTransform(banisters[1].transform);

        StairData[] stairDataArr = new StairData[stairs.Count];

        data.BanisterTFDataArr = banisterTFDataArr;

        for(int i = 0; i < stairs.Count; i++)
        {
            stairDataArr[i].TFData = Helper.CreateDataFromTransform(stairs[i].TF);
        }

        data.stairDataArr = stairDataArr;

        dataSO.BridgeDatas.Add(data);


        EditorUtility.SetDirty(dataSO);
        AssetDatabase.SaveAssets();

    }

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

        Helper.LoadTransformData(plane, bridgeData.TFPlane);

        SetOwnerStage(LevelManager.Instance.StageManager.GetStage(bridgeData.OwnerStageNumber));
        SetNextStage(LevelManager.Instance.StageManager.GetStage(bridgeData.NextStageNumber));

        for(int i = 0; i < bridgeData.stairDataArr.Length; i++)
        {
            StairData stairData = bridgeData.stairDataArr[i];

            Stair stair = SimplePool.Spawn<Stair>(PoolType.StairPool, Vector3.zero, Quaternion.identity);
            stair.TF.SetParent(tf, true);
            stair.OnInit();
            stair.LoadData(stairData);

            stairs.Add(stair);
        }

        foreach(Stair stair in stairs)
        {
            stair.SetBridge(this);
        }


    }

    public void OnInit()
    {
        stairs.Clear();
        ownerStage = null;
        nextStage = null;

    }

    public void OnDespawn()
    {
        for(int i = 0; i < stairs.Count; i++)
        {
            stairs[i].OnDeSpawn();
            SimplePool.Despawn(stairs[i]);
        }
        stairs.Clear();
        ownerStage = null;
        nextStage = null;
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

    

}

public struct StairInfo
{
    public int stairId;

    public bool isLastStair;
    public Vector3 position;
}
