using System.Collections.Generic;
using UnityEngine;

public class CanvasLevelSelect : UICanvas
{
    [SerializeField] private LevelUI levelUIPrefab;
    [SerializeField] private Transform levelContainer;

    [SerializeField] private RectTransform panelRoot;

    [SerializeField] private Vector2 targetPosition;

    [SerializeField] private float popUpDuration;

    [SerializeField] private List<LevelUI> listLevelUI = new List<LevelUI>();

    [SerializeField] private int selectedLevelId = -1;

    private bool isSetUp = false;

    public void OnDespawn()
    {
        for(int i = 0; i < listLevelUI.Count; i++)
        {
            Destroy(listLevelUI[i].gameObject);
        }
    }
    public override void SetUp()
    {
        Debug.Log(panelRoot.anchoredPosition);
        base.SetUp();
        EventBus<OnLevelSelect>.Subcribe(OnChangeLevelSelect);
        if(isSetUp)return;
        isSetUp = true;
        List<LevelData> levelDatas = GameData.Instance.AllLevelSaveData.LevelDatas;

        for(int i = 0; i < levelDatas.Count; i++)
        {
            LevelUI levelUI = Instantiate(levelUIPrefab, levelContainer);
            levelUI.SetUp();
            if(i > 0){
                levelUI.SetUp(levelDatas[i-1], levelDatas[i]);
            }
            else
            {
                levelUI.SetUp(new LevelData{}, levelDatas[i]);
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
        base.Close(time);
    }

    public void OnChangeLevelSelect(OnLevelSelect onLevelSelect)
    {
        selectedLevelId = onLevelSelect.LevelId;
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
