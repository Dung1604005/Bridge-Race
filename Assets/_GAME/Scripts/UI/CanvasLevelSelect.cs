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
        OnDespawn();
        panelRoot.anchoredPosition = startPosition;
        
        base.SetUp();
        
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
        
        OnDespawn();

        StartCoroutine(Helper.IEPopUp(panelRoot,startPosition, popUpDuration, 0.05f, () =>
        {
            base.Close(time);
        } ));
        
    }

    public void OnChangeLevelSelect(int levelId)
    {
        if(selectedLevelId == levelId)return;
        if(selectedLevelId >= 0 && selectedLevelId < listLevelUI.Count)
        {
            listLevelUI[selectedLevelId].OnDeSelect();
        }
        selectedLevelId = levelId;
    }

    public void OnCloseButton()
    {
        SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUTTON_CLICK);
        UIManager.Instance.CloseUI<CanvasLevelSelect>(0f);
  
    }

    public void PlayGame()
    {
        if(selectedLevelId == -1)
        {
            return;
        }
        SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUTTON_CLICK);
        LevelManager.Instance.LoadData(GameData.Instance.LevelDatas[selectedLevelId]);
        LevelManager.Instance.InitLevel();

        UIManager.Instance.CloseUI<CanvasMainMenu>(0f);
        UIManager.Instance.CloseUI<CanvasLevelSelect>(0f);

        UIManager.Instance.OpenUI<CanvasLoading>(this);
    }

    

}
