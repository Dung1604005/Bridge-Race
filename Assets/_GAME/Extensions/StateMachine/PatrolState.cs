using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private int numbTargetBrick = 0;

    public void CaculateAmountBrick(Enemy t)
    {
        int maxActiveBrick = t.CurrentStage.StageBrickManager.GetAmountActiveBrick(t.ColorType);
        
        numbTargetBrick = UnityEngine.Random.Range(Mathf.Min(3, maxActiveBrick), maxActiveBrick + 1);
    }

    public void SetDestination(Enemy t)
    {
        if(!t.IsAgentValid())
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
        if (t.IsAgentValid())
        {
            t.SetDestination(t.CurrentStage.StageBrickManager.GetNearestBrick(t.ColorType, t.TF.position));
        }
        
    }
    public void OnEnter(Enemy t)
    {
        t.ChangeAnim(GameConfig.ANIM_RUN);
        SetDestination(t);

    }

    public void OnExecute(Enemy t)
    {
        if (t.IsAgentStop() )
        {
            
            if(numbTargetBrick == 0)
            {
               OnExit(t);

               if(t.BrickCharacterManager.GetAmountVisualBrick() > 0)
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
                if(t.IsAgentValid())
                {
                    t.SetDestination(t.CurrentStage.StageBrickManager.GetNearestBrick(t.ColorType, t.TF.position));
                }
            }
        }
    }

    public void OnExit(Enemy t)
    {

    }

}
