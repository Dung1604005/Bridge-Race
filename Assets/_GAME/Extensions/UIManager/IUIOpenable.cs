using UnityEngine;

public interface IUIOpenable
{
    public void SetUp();

    public void Open();

    public void Close(float time);

    public void CloseDirectly();


}
