using System.Collections;
using TMPro;
using UnityEngine;

public class CanvasTimeUp : UICanvas
{

    [SerializeField] private RectTransform timeUpEffect;

    [SerializeField] private Vector2 targetTimeUpPos;

    [SerializeField] private Vector2 originTimeUpPos;

    [ContextMenu("PRINT POSITION")]
    public void PrintPosition()
    {
        Debug.Log(timeUpEffect.position);
    }
    public override void SetUp()
    {
        base.SetUp();
        PlayEffect(true);        
    }

    public void PlayEffect(bool isStart)
    {
        Vector2 target = isStart ? targetTimeUpPos: originTimeUpPos;
        Debug.Log(target);
        StartCoroutine(Helper.IEPopUp(timeUpEffect, targetTimeUpPos, 0.5f, 0.05f));
    }
}
