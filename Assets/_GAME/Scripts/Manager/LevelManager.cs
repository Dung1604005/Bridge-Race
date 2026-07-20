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
    [SerializeField] private List<ColorType> listColors = new List<ColorType>() { ColorType.RED, ColorType.BLUE, ColorType.VIOLET, ColorType.GREEN };

    [SerializeField] private List<Character> listCharacter = new List<Character>();

    [SerializeField] private StageManager stageManager;

    [SerializeField] private RankManager rankManager;

    [SerializeField] private CameraFollow cam;

    [SerializeField] private WinArea winArea;

    public Transform LevelRoot => levelRoot;

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

    public void InitLevel()
    {
        rankManager.LoadRankedList(listCharacter);

        stageManager.OnInit();
        stageManager.LoadStage(levelDataSO.stageDatas);
        InitDecorObject();
        InitGate();
        InitWinArea();
        stageManager.BakeNavMeshSurface(levelDataSO);
        InitCharacter();
    }

    public void InitWinArea()
    {
        winArea.OnInit();
        winArea.LoadData(levelDataSO.WinAreaTF);
    }

    public void InitGate()
    {
        for (int i = 0; i < levelDataSO.gateDatas.Count; i++)
        {
            GateCtrl gate = SimplePool.Spawn<GateCtrl>(PoolType.GatePool, Vector3.zero, Quaternion.identity);

            gate.TF.SetParent(gateRoot, true);
            gate.OnInit();
            gate.LoadData(levelDataSO.gateDatas[i]);
        }

    }

    public void InitCharacter()
    {
        foreach (Character character in listCharacter)
        {
            character.ChangeStage(stageManager.GetStage(1));
            character.ReSpawn();
        }


    }

    public void InitDecorObject(){

        for(int i = 0; i < levelDataSO.decorObjectDatas.Count; i++)
        {
            DecorData decorData = levelDataSO.decorObjectDatas[i];
            GameObject decor = Instantiate(GameData.Instance.listDecorObject[decorData.DecorId]);

            decor.transform.SetParent(decorRoot, true);
            Helper.LoadTransformData(decor.transform, decorData.TFData);
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
        cam.OnWin();
    }

    public Vector3 GetWinAreaPosition()
    {
        return winArea.TF.position;
    }
    void Awake()
    {
        
        InitLevel();
    }

    void Start()
    {
        UIManager.Instance.GetUI<CanvasGamePlay>().SetRankUI(rankManager.GetRankedList());
    }

}


