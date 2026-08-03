using UnityEngine;

public class WiningChaseState : IState
{
    public void OnEnter(Enemy t)
    {
        t.ChangeAnim(GameData.Instance.ANIM_RUN);
        if (t.IsAgentValid())
        {
            t.SetDestination(LevelManager.Instance.GetWinAreaPosition());
        }
        
    }

    public void OnExecute(Enemy t)
    {

    }

    public void OnExit(Enemy t)
    {

    }
}
