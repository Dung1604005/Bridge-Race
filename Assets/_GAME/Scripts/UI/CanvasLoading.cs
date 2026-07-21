using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLoading : UICanvas
{
    [SerializeField] private float loadingTime;

    [SerializeField] private Slider loadingSlider;

    [SerializeField] private TextMeshProUGUI textLoading;

    private UICanvas parentCanvas;

    public override void Open(UICanvas uICanvas)
    {
        parentCanvas = uICanvas;
        base.Open(uICanvas);
        
        StartCoroutine(IELoading());
    }

    private IEnumerator IELoading()
    {
        float timer  = 0f;
        while(timer + 0.01f < loadingTime)
        {
            timer += Time.deltaTime;

            loadingSlider.value = timer/loadingTime;

            int percent = (int)Math.Floor(timer*100/loadingTime);
            textLoading.text = "Loading..." + percent + "%";
            yield return null;
        }

        if(parentCanvas is CanvasLevelSelect || parentCanvas is CanvasSettings)
        {
            
            UIManager.Instance.CloseUIDirectly<CanvasLoading>();
            CanvasGamePlay canvasGamePlay = UIManager.Instance.OpenUI<CanvasGamePlay>();

            
            canvasGamePlay.StartCountDown();
            
            UIManager.Instance.GetUI<CanvasGamePlay>().SetRankUI(LevelManager.Instance.RankManager.GetRankedList());
        }
    }


}
