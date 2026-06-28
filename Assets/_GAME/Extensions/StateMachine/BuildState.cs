using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{

    private Bridge bestBridge;

    private int stairId;

    private bool canReachLastStair;

    public void CaculateDestination(Enemy t)
    {
        int amountBrick = t.GetAmountBrick();
        
        if (amountBrick > 0)
        {
            StairInfo stairInfo = bestBridge.GetFarthestStairPossible(-1, t.ColorType, amountBrick);
            if (stairId == stairInfo.stairId)
            {
                OnExit(t);
                
                t.ChangeState(new PatrolState());
                return;
            }
            stairId = stairInfo.stairId;
            t.Agent.SetDestination(stairInfo.position);
        }
        else
        {

            OnExit(t);
            t.ChangeState(new PatrolState());
            
        }
    }

    public void OnEnter(Enemy t)
    {
        bestBridge = t.CurrentStage.GetBestBridge(t.ColorType);
        CaculateDestination(t);
    }


    public void OnExecute(Enemy t)
    {

        if (t.IsAgentStop())
        {
            
            CaculateDestination(t);

        }
    }

    public void OnExit(Enemy t)
    {

    }

}
