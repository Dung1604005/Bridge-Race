using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{

    private Bridge bestBridge;

    private int stairId = -1;

    private bool canReachLastStair;

    private Enemy enemy;

    public void ReCaculate(OnStairChange onStairChange)
    {
        if(enemy.CharacterId == onStairChange.CharacterId)
        {
           stairId = onStairChange.StairId;

           CaculateDestination();

        }
    }

    public void CaculateDestination()
    {
        int amountBrick = enemy.BrickCharacterManager.GetAmountVisualBrick();
        StairInfo stairInfo = bestBridge.GetFarthestStairPossible(stairId, enemy.ColorType, amountBrick);

        if (stairId == stairInfo.stairId)
        {
            OnExit(enemy);
            if (stairInfo.isLastStair)
            {


                if (bestBridge.OwnerStage.IsLastStage)
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
            stairId = stairInfo.stairId;
            if (enemy.Agent.enabled && enemy.Agent.isOnNavMesh)
            {
                enemy.Agent.SetDestination(stairInfo.position);
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
        EventBus<OnStairChange>.Subcribe(ReCaculate);
        enemy = t;
        bestBridge = t.CurrentStage.GetBestBridge(t.ColorType);
        CaculateDestination();
    }


    public void OnExecute(Enemy t)
    {

        if (t.IsAgentStop())
            CaculateDestination();


    }

    public void OnExit(Enemy t)
    {
        EventBus<OnStairChange>.UnSubcribe(ReCaculate);

    }

}
