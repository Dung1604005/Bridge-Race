using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{

    public void CaculateAmountBrick(Enemy t)
    {
        if(!t.IsAgentValid())
        {
            return;
        }
        int maxActiveBrick = t.CurrentStage.StageBrickManager.GetAmountActiveBrick(t.ColorType);
        int numbTargetBrick = UnityEngine.Random.Range(Mathf.Min(3, maxActiveBrick), maxActiveBrick + 1);
        
        t.SetNumbTargetBrick(numbTargetBrick);
    }

    public void SetDestination(Enemy t)
    {
        if(!t.IsAgentValid())
        {
            return;
        }
         CaculateAmountBrick(t);
        if(t.GetNumbTargetBrick() == 0)
        {
            t.ChangeState(new IdleState());
            return;
        }
        t.SetNumbTargetBrick(t.GetNumbTargetBrick() - 1);
        if (t.IsAgentValid())
        {
            t.SetDestination(t.CurrentStage.StageBrickManager.GetNearestBrick(t.ColorType, t.TF.position));
        }
        
    }
    public void OnEnter(Enemy t)
    {
        t.SetNumbTargetBrick(0);
        t.ChangeAnim(GameConfig.ANIM_RUN);
        SetDestination(t);

    }

    public void OnExecute(Enemy t)
    {
        if (t.IsAgentStop() )
        {
            
            if(t.GetNumbTargetBrick()  == 0)
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
                t.SetNumbTargetBrick(t.GetNumbTargetBrick() - 1);
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
