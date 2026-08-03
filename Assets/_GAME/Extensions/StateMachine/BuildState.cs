using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{

    private Bridge bestBridge;
    private Enemy enemy;

    public void ReCaculate(OnStairChange onStairChange)
    {
        if(enemy.CharacterId == onStairChange.CharacterId)
        {
           enemy.SetStairId(onStairChange.StairId);

           CaculateDestination();

        }
    }

    public void CaculateDestination()
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
