using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{

    private Bridge bestBridge;

    public void CaculateDestination(Enemy enemy)
    {
        int amountBrick = enemy.BrickCharacterManager.GetAmountVisualBrick();
        StairInfo stairInfo = bestBridge.GetFarthestStairPossible(enemy.GetStairId(), enemy.ColorType, amountBrick);

        if (enemy.GetStairId() == stairInfo.stairId)
        {
            OnExit(enemy);
            if (stairInfo.isLastStair)
            {
                if (bestBridge.OwnerStage.IsLastStage())
                {
                    enemy.ChangeState(new WiningChaseState());
                    return;
                }
                else
                {
                    enemy.ChangeStage(bestBridge.NextStage);
                }
            }
            enemy.ChangeState(new PatrolState());
            return;
        }
        if (amountBrick > 0)
        {
            enemy.SetStairId(stairInfo.stairId);
            if (enemy.IsAgentValid())
            {
                enemy.SetDestination(stairInfo.position);
            }
        }
        else
        {

            OnExit(enemy);
            enemy.ChangeState(new PatrolState());

        }
    }

    public void OnEnter(Enemy t)
    {
        t.SetStairId(-1);
        bestBridge = t.CurrentStage.GetBestBridge(t.ColorType);
        CaculateDestination(t);
    }


    public void OnExecute(Enemy t)
    {

        if (t.IsAgentStop())
            CaculateDestination(t);


    }

    public void OnExit(Enemy t)
    {

    }

}
