using UnityEngine;

public class Brick : GameUnit
{
    private Stage stage;

    
    public void OnInit(Stage _stage)
    {
        stage = _stage;
        

    }

    public void OnDespawn()
    {
        stage = null;
        SimplePool.Despawn(this);
    }

    void Awake()
    {
        tf = this.transform;
    }
}
