using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private List<Stage> stages = new List<Stage>();

    [SerializeField] private Transform stageRoot;

    [SerializeField] private NavMeshSurface navMeshSurface;

    public List<Stage> Stages => stages;

    

    public void LoadStage(List<StageDataSO> datas)
    {
        stages.Clear();
        for(int i = 0; i < datas.Count; i++)
        {
            Stage stage = SimplePool.Spawn<Stage>(PoolType.StagePool, Vector3.zero, Quaternion.identity);
            stage.TF.SetParent(LevelManager.Instance.StageRoot, true);
            stage.OnInit();
            stage.LoadData(datas[i]);
        }
    }

    public void BakeNavMeshSurface()
    {
        navMeshSurface.BuildNavMesh();
    }

    public Stage GetStage(int stageNumber)
    {
        for(int i = 0; i < stages.Count; i++)
        {
            if(stages[i].StageNumber == stageNumber)
            {
                return stages[i];
            }
        }
        Debug.LogError("STAGE " + stageNumber + " DONT EXIST IN STAGE MANAGER");
        return null;
    }
}
