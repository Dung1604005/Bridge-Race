using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class DynamicJoyStick : OnScreenControl, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("UI REF")]

    [SerializeField] private RectTransform joystick;

    [SerializeField] private RectTransform handle;

    [Header("THONG SO")]

    [SerializeField] private float moveRanger;

    [InputControl(layout = "Vector2")]

    [SerializeField] private string controlPath;

    protected override string controlPathInternal { 
        get => controlPath
    ; set => controlPath = value; }


    void Awake()
    {
        joystick.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        joystick.gameObject.SetActive(true);

        joystick.position = pointerEventData.position;
        handle.anchoredPosition = Vector2.zero;

        SendValueToControl(Vector2.zero);

    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        
        

        Vector2 delta = (Vector2)pointerEventData.position - (Vector2)joystick.position;

        
        if(delta.sqrMagnitude > moveRanger * moveRanger)
        {
            delta = delta.normalized*moveRanger;
        }

        handle.anchoredPosition = delta;
        Debug.Log(delta/moveRanger);
        SendValueToControl(delta/moveRanger);
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        joystick.gameObject.SetActive(false);

        
        SendValueToControl(Vector2.zero);
    }
}
