using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelDataSO levelDataSO;

    [SerializeField] private Transform stageRoot;
    [SerializeField] private Transform levelRoot;
    [SerializeField] private List<ColorType> listColors = new List<ColorType>(){ColorType.RED, ColorType.BLUE, ColorType.VIOLET, ColorType.GREEN};

    [SerializeField] private List<Character> listCharacter = new List<Character>();

    [SerializeField] private StageManager stageManager;

    [SerializeField] private RankManager rankManager;

    [SerializeField] private CameraFollow cam;

    [SerializeField] private WinArea winArea;

    public Transform LevelRoot => levelRoot;

    public Transform StageRoot => stageRoot;

    public RankManager RankManager => rankManager;

    public List<ColorType> ListColors => listColors;
    public Vector3 GetWinAreaPosition()
    {
        return winArea.TF.position;
    }

    public void InitLevel()
    {
        stageManager.LoadStage(levelDataSO.stageDatas);
        InitCharacter();

        
    }

    public void InitCharacter()
    {
        foreach (Character character in listCharacter)
        {
            character.ChangeStage(stageManager.GetStage(1));
            character.ReSpawn();
        }


    }

    public void OnWin()
    {
        foreach(Character character in listCharacter)
        {
            character.OnWin();
        }

        foreach(Stage stage in stageManager.Stages)
        {
            stage.OnWin();
        }
        cam.OnWin();
    }
    void Awake()
    {
        rankManager.LoadRankedList(listCharacter);
        InitLevel();
    }

    void Start()
    {
        UIManager.Instance.GetUI<CanvasGamePlay>().SetRankUI(rankManager.GetRankedList());
    }

}

