using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<Stage> stages = new List<Stage>();
    [SerializeField] private NavMeshSurface navMeshSurface;

    public void OnInit()
    {
        stages.Clear();
    }
    public void OnDespawn()
    {
        for(int i = 0; i < stages.Count; i++)
        {
            stages[i].OnDespawn();
            SimplePool.Despawn(stages[i]);
        }
        stages.Clear();
    }
    public void LoadStage(List<StageDataSO> datas)
    {
        
        for(int i = 0; i < datas.Count; i++)
        {
            Stage stage = SimplePool.Spawn<Stage>(PoolType.StagePool, Vector3.zero, Quaternion.identity);
            stage.TF.SetParent(LevelManager.Instance.StageRoot, true);
            stages.Add(stage);
            stages[i].OnInit();
            stages[i].LoadData(datas[i]);

            
        }

        for(int i = 0; i < datas.Count; i++)
        {
            stages[i].LoadDataBridge(datas[i]);
        }
    }

    public void BakeNavMeshSurface(LevelDataSO levelDataSO)
    {
        navMeshSurface.navMeshData = levelDataSO.NavMeshData;   
        navMeshSurface.AddData();
    }

    public Stage GetStage(int stageNumber)
    {
        for(int i = 0; i < stages.Count; i++)
        {
            if(stages[i].GetStageNumber() == stageNumber)
            {
                return stages[i];
            }
        }
        Debug.Log("STAGE " + stageNumber + " DONT EXIST IN STAGE MANAGER");
        return null;
    }

    public List<Stage> GetAllStage()
    {
        return stages;
    }
}
