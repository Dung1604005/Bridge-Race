using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private int numbTargetBrick = 0;

    public void CaculateAmountBrick(Enemy t)
    {
        int maxActiveBrick = t.CurrentStage.GetAmountActiveBrick(t.ColorType);
        
        numbTargetBrick = UnityEngine.Random.Range(Mathf.Min(3, maxActiveBrick), maxActiveBrick + 1);
    }

    public void SetDestination(Enemy t)
    {
        if(t.Agent.enabled == false)
        {
            return;
        }
         CaculateAmountBrick(t);
        if(numbTargetBrick == 0)
        {
            t.ChangeState(new IdleState());
            return;
        }
        numbTargetBrick -= 1;
        t.Agent.SetDestination(t.CurrentStage.GetNearestBrick(t.ColorType, t.TF.position));
        t.ChangeAnim(GameData.Instance.ANIM_RUN);
    }
    public void OnEnter(Enemy t)
    {
        SetDestination(t);

    }

    public void OnExecute(Enemy t)
    {
        if (t.IsAgentStop() )
        {
            
            if(numbTargetBrick == 0)
            {
               OnExit(t);

               if(t.GetAmountBrick() > 0)
                {
                    t.ChangeState(new BuildState()); 
                }
                else
                {
                    SetDestination(t);
                }
            }
            else
            {
                numbTargetBrick -= 1;
                if(t.Agent.enabled == true)
                {
                    t.Agent.SetDestination(t.CurrentStage.GetNearestBrick(t.ColorType, t.TF.position));
                }
            }
        }
    }

    public void OnExit(Enemy t)
    {

    }

}
