

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class Stage : GameUnit
{

    [SerializeField] private StageDataSO stageDataSO;
    [SerializeField] private int stageNumber;

    [SerializeField]private bool isLastStage;

    [SerializeField] private StageBrickManager stageBrickManager;


    [SerializeField] private List<Transform> spawnPos = new List<Transform>();

    [SerializeField] private List<Character> characters = new List<Character>();

    [SerializeField] private List<Bridge> bridges = new List<Bridge>();

    public List<Character> Characters => characters;

    public StageBrickManager StageBrickManager => stageBrickManager;

    [ContextMenu("Create Data")]

    public void CreateData()
    {
        StageData stageData = new StageData();

        stageData.TFData = Helper.CreateDataFromTransform(tf);

        Debug.Log(stageData.TFData.EulerAngles);

        stageData.StageNumber = stageNumber;

        stageData.IsLastStage = isLastStage;
        stageData.ScaleX = stageBrickManager.ScaleX;
        stageData.ScaleY = stageBrickManager.ScaleY;
        stageData.ScaleZ = stageBrickManager.ScaleZ;


        TransformData[] SpawnPos = new TransformData[spawnPos.Count];
        for(int i = 0; i < SpawnPos.Length; i++)
        {
            SpawnPos[i] = Helper.CreateDataFromTransform(spawnPos[i]);
        }

        stageData.SpawnPos = SpawnPos;

        stageDataSO.SetStageData(stageData);

        EditorUtility.SetDirty(stageDataSO);
        AssetDatabase.SaveAssets();

        
    }

    public void LoadData(StageDataSO stageDataSO)
    {
        Helper.LoadTransformData(tf, stageDataSO.StageData.TFData);

        isLastStage = stageDataSO.StageData.IsLastStage;

        stageNumber = stageDataSO.StageData.StageNumber;
        stageBrickManager.SetScale(stageDataSO.StageData.ScaleX, stageDataSO.StageData.ScaleY, stageDataSO.StageData.ScaleZ);

        for(int i = 0; i < stageDataSO.StageData.SpawnPos.Length; i++)
        {
            Helper.LoadTransformData(spawnPos[i], stageDataSO.StageData.SpawnPos[i]);
        }
        stageBrickManager.SpawnBrickStage(LevelManager.Instance.ListColors);
    }

    public void LoadDataBridge(StageDataSO stageDataSO)
    {
        for(int i = 0; i < stageDataSO.BridgeDatas.Count; i++)
        {
            TransformData tfData = stageDataSO.BridgeDatas[i].bridgeTFData;

            Bridge newBridge= SimplePool.Spawn<Bridge>(PoolType.BridgePool, tfData.Position, Quaternion.identity);
            newBridge.TF.SetParent(LevelManager.Instance.LevelRoot, true);
            newBridge.OnInit();
            newBridge.LoadData(stageDataSO.BridgeDatas[i]);
            bridges.Add(newBridge);
            
        }
    }
    public void OnInit()
    {      
        characters.Clear();
        bridges.Clear();
        stageBrickManager.ClearAllBrick();
    }

    public void OnDespawn()
    {
        characters.Clear();
        for(int i = 0; i < bridges.Count; i++)
        {
            bridges[i].OnDespawn();
            SimplePool.Despawn(bridges[i]);
        }
        bridges.Clear();
        stageBrickManager.ClearAllBrick();
    }

    public void OnWin()
    {
        foreach(List<Brick> listBrick in stageBrickManager.Bricks.Values)
        {
            foreach(Brick brick in listBrick)
            {
                brick.OnWin();
            }
        }
    }

    public int GetStageNumber()
    {
        return stageNumber;
    }

    public bool IsLastStage()
    {
        return isLastStage;
    }

    public Vector3 GetSpawnPosCharacter(Character character)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if(character == characters[i])
            {
                return spawnPos[i].position;

            }
        }
        
        return spawnPos[0].position;
    }

    public void AddCharacter(Character character)
    {
        characters.Add(character);
        stageBrickManager.ActiveBrickByColor(character.ColorType);

    }

    public void RemoveCharacter(Character character)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] == character)
            {
                stageBrickManager.DeActiveBrickByColor(character.ColorType);
                characters.RemoveAt(i);
               

                return;
            }
        }
    }


    public Bridge GetBestBridge(ColorType colorType)
    {
        int maxColorStair = 0;
        List<Bridge> possibleAns = new List<Bridge>();
        for (int i = 0; i < bridges.Count; i++)
        {
            int amountColorStair = bridges[i].GetAmountColorStair(colorType);
            if (amountColorStair > maxColorStair)
            {
                maxColorStair = amountColorStair;
                possibleAns.Clear();
                possibleAns.Add(bridges[i]);
            }
            else if (amountColorStair == maxColorStair)
            {
                possibleAns.Add(bridges[i]);
            }
        }
        if (possibleAns.Count == 1)
        {
            return possibleAns[0];
        }
        else
        {
            int rad = UnityEngine.Random.Range(0, possibleAns.Count);
            return possibleAns[rad];
        }

    }

    

    void Awake()
    {
        tf = this.transform;

    }

    void Update()
    {
        if(GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }
        stageBrickManager.UpdateBrickCollection();


    }
}


