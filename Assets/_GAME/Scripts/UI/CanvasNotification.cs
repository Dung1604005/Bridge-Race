using System;
using TMPro;
using UnityEngine;

public class CanvasNotification : UICanvas
{
    [SerializeField] private TextMeshProUGUI textTitle;

    [SerializeField] private TextMeshProUGUI textContent;

    [SerializeField] private RectTransform popUpRoot;

    [SerializeField] private Vector2 startPosition;

    [SerializeField] private Vector2 targetPosition;

    [SerializeField] private float popUpDuration;

    public override void SetUp()
    {
        base.SetUp();
        this.gameObject.SetActive(true);
        
        popUpRoot.position = startPosition;
        Debug.Log(this.isActiveAndEnabled);
        StartCoroutine(Helper.IEPopUp(popUpRoot,targetPosition, popUpDuration, 0.05f ));
    }

    public void SetText(String title, String content)
    {
        textTitle.text = title;
        textContent.text = content;
    }

    public void OnButtonClose()
    {
        UIManager.Instance.CloseUIDirectly<CanvasNotification>();
    }
}
