using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelDataSO levelDataSO;

    [SerializeField] private Transform stageRoot;
    [SerializeField] private Transform levelRoot;

    [SerializeField] private Transform gateRoot;

    [SerializeField] private Transform decorRoot;

    [SerializeField] private List<GateCtrl> listGateCtrl = new List<GateCtrl>(); 

    [SerializeField] private List<GameObject> decorList = new List<GameObject>();
    [SerializeField] private List<ColorType> listColors = new List<ColorType>() { ColorType.RED, ColorType.BLUE, ColorType.VIOLET, ColorType.GREEN };

    [SerializeField] private List<Character> listCharacter = new List<Character>();

    [SerializeField] private StageManager stageManager;

    [SerializeField] private RankManager rankManager;

    [SerializeField] private CameraFollow cam;

    [SerializeField] private CameraFollow camUIObject;

    [SerializeField] private WinArea winArea;

    public Transform LevelRoot => levelRoot;

    public LevelDataSO LevelDataSO => levelDataSO;

    public Transform StageRoot => stageRoot;

    public RankManager RankManager => rankManager;

    public StageManager StageManager => stageManager;

    public List<ColorType> ListColors => listColors;

    [ContextMenu("CREATE DECOR + WinArea DATA")]
    public void CreateDecorData()
    {
        List<DecorData> decorObjectDatas = new List<DecorData>();
        foreach(Transform child in decorRoot)
        {
            DecorData decorData = new DecorData();
            decorData.TFData = Helper.CreateDataFromTransform(child);
            int decorId = child.GetComponent<DecorObject>().DecorId;      
            decorData.DecorId = decorId;
            decorObjectDatas.Add(decorData);            
        }

        levelDataSO.decorObjectDatas = decorObjectDatas;

        TransformData winAreaTFData = Helper.CreateDataFromTransform(winArea.TF);
        levelDataSO.WinAreaTF = winAreaTFData;
        EditorUtility.SetDirty(levelDataSO);
        AssetDatabase.SaveAssets();
    }

    public void LoadData(LevelDataSO levelDataSO)
    {
        this.levelDataSO = levelDataSO;
    }

    public void StartGame()
    {
        for(int i = 0; i < listCharacter.Count; i++)
        {
            listCharacter[i].OnStart();
        }
    }

    public void InitColor()
    {
        int randomSeed = (int)Helper.GetRandomColor();

        for(int i = 0; i < listColors.Count; i++)
        {
            listColors[i] = (ColorType)(randomSeed + i);
        }
    }

    public void InitLevel()
    {
        InitColor();
        rankManager.LoadRankedList(listCharacter);
        stageManager.OnInit();
        stageManager.LoadStage(levelDataSO.stageDatas);
        InitDecorObject();
        InitGate();
        InitWinArea();
        stageManager.BakeNavMeshSurface(levelDataSO);
        InitCharacter();      
        cam.OnInit();
        camUIObject.OnInit();
    }
    public void DeSpawnLevel()
    {
        DeSpawnCharacter();
        DeSpawnWinArea();
        DeSpawnGate();
        DespawnDecorObject();
        stageManager.OnDespawn();
    }

    public void InitWinArea()
    {
        winArea.OnInit();
        winArea.LoadData(levelDataSO.WinAreaTF);

    }

    public void DeSpawnWinArea()
    {
        winArea.OnDespawn();
    }

    public void InitGate()
    {
        listGateCtrl.Clear();
        for (int i = 0; i < levelDataSO.gateDatas.Count; i++)
        {
            GateCtrl gate = SimplePool.Spawn<GateCtrl>(PoolType.GatePool, Vector3.zero, Quaternion.identity);

            gate.TF.SetParent(gateRoot, true);
            gate.OnInit();
            gate.LoadData(levelDataSO.gateDatas[i]);

            listGateCtrl.Add(gate);
        }

    }

    public void DeSpawnGate()
    {
        for(int i = 0; i < listGateCtrl.Count; i++)
        {
            listGateCtrl[i].OnDespawn();
            SimplePool.Despawn(listGateCtrl[i]);
        }
        
    }

    public void InitCharacter()
    {
        for(int i = 0; i < listCharacter.Count; i++)
        {
            Character character = listCharacter[i];
            character.SetColor(listColors[i]);
            character.ChangeStage(stageManager.GetStage(1), false);
            character.ReSpawn();
            if(character is PlayerController)
            {
                character.SetSpeed(levelDataSO.SpeedPlayer);
            }
            else
            {
                character.SetSpeed(levelDataSO.SpeedBot);
            }
        }
        
    }

    public void DeSpawnCharacter()
    {
         foreach (Character character in listCharacter)
        {
            character.OnDespawn();
        }
    }

    public void InitDecorObject(){

        for(int i = 0; i < levelDataSO.decorObjectDatas.Count; i++)
        {
            DecorData decorData = levelDataSO.decorObjectDatas[i];
            GameObject decor = Instantiate(GameData.Instance.listDecorObject[decorData.DecorId]);

            decor.transform.SetParent(decorRoot, true);
            Helper.LoadTransformData(decor.transform, decorData.TFData);

            decorList.Add(decor);


        }
    }

    public void DespawnDecorObject()
    {
        for(int i = 0; i < decorList.Count; i++)
        {
            Destroy(decorList[i]);
        }
    }

    public void OnPause()
    {
        foreach(Character character in listCharacter)
        {
            character.OnPause();
        }
    }

    public void OnContinue()
    {
        foreach(Character character in listCharacter)
        {
            character.OnContinue();
        }
    }

    public void OnWin()
    {
        foreach (Character character in listCharacter)
        {
            character.OnWin();
        }

        foreach (Stage stage in stageManager.Stages)
        {
            stage.OnWin();
        }
        int totalStar = rankManager.CaculateStarPlayer();
        if(totalStar > 0)
        {
            GameData.Instance.SaveLevel(new LevelDataSave
            {
                LevelId = levelDataSO.LevelId,
                TotalStar = totalStar
            });
        }
        cam.OnWin();
        camUIObject.OnWin();
    }

    public Vector3 GetWinAreaPosition()
    {
        return winArea.TF.position;
    }
}


