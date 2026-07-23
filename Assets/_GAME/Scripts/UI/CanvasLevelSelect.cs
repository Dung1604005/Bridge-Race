using System.Collections.Generic;
using UnityEngine;

public class CanvasLevelSelect : UICanvas
{
    [SerializeField] private LevelUI levelUIPrefab;
    [SerializeField] private Transform levelContainer;

    [SerializeField] private RectTransform panelRoot;

    [SerializeField] private Vector2 startPosition;

    [SerializeField] private Vector2 targetPosition;

    [SerializeField] private float popUpDuration;

    [SerializeField] private List<LevelUI> listLevelUI = new List<LevelUI>();

    [SerializeField] private int selectedLevelId = -1;


    public void OnDespawn()
    {
        for(int i = 0; i < listLevelUI.Count; i++)
        {
            Destroy(listLevelUI[i].gameObject);
        }
        listLevelUI.Clear();
    }
    public override void SetUp()
    {
        panelRoot.anchoredPosition = startPosition;
        
        base.SetUp();
        EventBus<OnLevelSelect>.Subcribe(OnChangeLevelSelect);
        List<LevelDataSave> levelDatas = GameData.Instance.AllLevelSaveData.LevelDatas;

        for(int i = 0; i < levelDatas.Count; i++)
        {
            LevelUI levelUI = Instantiate(levelUIPrefab, levelContainer);
            levelUI.SetUp();
            if(i > 0){
                levelUI.SetUp(levelDatas[i-1], levelDatas[i]);
            }
            else
            {
                levelUI.SetUp(new LevelDataSave{}, levelDatas[i]);
            }

            listLevelUI.Add(levelUI);
            
        }
        
    }

    public override void Open()
    {
        base.Open();
        StartCoroutine(Helper.IEPopUp(panelRoot,targetPosition, popUpDuration, 0.05f ));
    }

    public override void Close(float time)
    {
        EventBus<OnLevelSelect>.UnSubcribe(OnChangeLevelSelect);
        OnDespawn();

        StartCoroutine(Helper.IEPopUp(panelRoot,startPosition, popUpDuration, 0.05f, () =>
        {
            base.Close(time);
        } ));
        
    }

    public void OnChangeLevelSelect(OnLevelSelect onLevelSelect)
    {
        selectedLevelId = onLevelSelect.LevelId;
    }

    public void OnCloseButton()
    {
        UIManager.Instance.CloseUI<CanvasLevelSelect>(0f);


        
    }

    public void PlayGame()
    {
        if(selectedLevelId == -1)
        {
            return;
        }
        LevelManager.Instance.LoadData(GameData.Instance.LevelDatas[selectedLevelId]);
        LevelManager.Instance.InitLevel();

        UIManager.Instance.CloseUIDirectly<CanvasMainMenu>();
        UIManager.Instance.CloseUIDirectly<CanvasLevelSelect>();

        UIManager.Instance.OpenUI<CanvasLoading>(this);
    }

    

}
