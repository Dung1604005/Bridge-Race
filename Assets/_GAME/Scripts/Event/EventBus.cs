using System;

public static class EventBus<T> where T: IEvent
{
    
    public static event Action<T> OnAction;

    public static void Subcribe(Action<T> handler)
    {
        OnAction += handler;
    }

    public static void UnSubcribe(Action<T> handler)
    {
        OnAction -= handler;
    }

    public static void Raise(T eventData)
    {
        OnAction?.Invoke(eventData);
    }
}