using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : UIElement
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private List<Image> imageStars = new List<Image>();

    [SerializeField] private GameObject selectEffect;

    [SerializeField] private float durationEffect;

    [SerializeField] private Vector3 scaleMin;

    [SerializeField] private Vector3 scaleMax;

    [SerializeField] private bool isSelected;

    [SerializeField] private bool isUnlocked;

    [SerializeField] private int levelIndex;


    public override void SetUp()
    {
        base.SetUp();
        selectEffect.gameObject.SetActive(false);
        button.interactable = false;
        isSelected = false;
        isUnlocked = false;
        levelText.text = "0";
        foreach(Image image in imageStars)
        {
            image.gameObject.SetActive(false);
        }
        
    }

    public void SetUp(LevelDataSave prevLevel, LevelDataSave levelData)
    {
        levelText.text = (levelData.LevelId + 1).ToString();
        levelIndex = levelData.LevelId;
        if(levelData.LevelId != 0 && prevLevel.TotalStar == 0)
        {
            return;
        }
        isUnlocked = true;
        button.interactable = true;

        for(int i = 0; i < levelData.TotalStar; i++)
        {
            imageStars[i].gameObject.SetActive(true);
        }
    }

    public void OnDeSelect()
    {
        isSelected = false;
        selectEffect.SetActive(false);
    }

    public void OnClick(float durationEffect)
    {
        SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUTTON_CLICK);
        isSelected = true;
        selectEffect.SetActive(true);
        EventBus<OnLevelSelect>.Raise(new OnLevelSelect{LevelId = levelIndex});
        StartCoroutine(IEPlayEffectClick(durationEffect));
    }

    IEnumerator IEPlayEffectClick(float duration)
    {
        StartCoroutine(Helper.IEDoScale(this.transform, scaleMin, durationEffect/4f));

        yield return new WaitForSeconds(duration/4f);

        StartCoroutine(Helper.IEDoScale(this.transform, scaleMax, durationEffect/2f));

        yield return new WaitForSeconds(duration/2f);

        StartCoroutine(Helper.IEDoScale(this.transform, Vector3.one, durationEffect/4f));


    }

   
}
