using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<ColorType> listColors = new List<ColorType>(){ColorType.RED, ColorType.BLUE, ColorType.VIOLET, ColorType.GREEN};

    [SerializeField] private List<Character> listCharacter = new List<Character>();

    [SerializeField] private List<Stage> listStage = new List<Stage>();

    [SerializeField] private RankManager rankManager;

    [SerializeField] private CameraFollow cam;

    [SerializeField] private WinArea winArea;

    public RankManager RankManager => rankManager;

    public List<ColorType> ListColors => listColors;
    public Vector3 GetWinAreaPosition()
    {
        return winArea.TF.position;
    }

    public void InitStage()
    {
        foreach (Stage stage in listStage)
        {
            stage.OnInit();

        }

        
    }

    public void InitCharacter()
    {
        foreach (Character character in listCharacter)
        {
            character.ChangeStage(listStage[0]);
            character.ReSpawn();
        }


    }

    public void OnWin()
    {
        foreach(Character character in listCharacter)
        {
            character.OnWin();
        }

        foreach(Stage stage in listStage)
        {
            stage.OnWin();
        }
        cam.OnWin();
    }
    void Awake()
    {
        rankManager.LoadRankedList(listCharacter);
        InitStage();
        InitCharacter();
    }

    void Start()
    {
        UIManager.Instance.GetUI<CanvasGamePlay>().SetRankUI(rankManager.GetRankedList());
    }

}

