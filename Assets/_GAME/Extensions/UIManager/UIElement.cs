using UnityEngine;

public class UIElement : MonoBehaviour, IUIOpenable
{
    [SerializeField] bool IsDestroyOnClose = false;


    /// <summary>
    /// Call beforce canvas is active
    /// </summary>
    public virtual void SetUp()
    {
        
    }

    /// <summary>
    /// Call after canvas is active
    /// </summary>
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }


    /// <summary>
    /// Close canvas after time(s)
    /// </summary>
    /// <param name="time"></param>
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDirectly), time);
    }

    /// <summary>
    /// Close canvas directly
    /// </summary>
    public virtual void CloseDirectly()
    {
        if (IsDestroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
}
