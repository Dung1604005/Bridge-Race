using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public void OnEnter(Enemy t)
    {
        t.ChangeAnim(GameConfig.ANIM_IDLE);
        
    }

    public void OnExecute(Enemy t)
    {

    }

    public void OnExit(Enemy t)
    {

    }

}
