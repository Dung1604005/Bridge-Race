using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<Stage> stages = new List<Stage>();

    [SerializeField] private Transform stageRoot;

    [SerializeField] private NavMeshSurface navMeshSurface;

    public List<Stage> Stages => stages;

    public void OnInit()
    {
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
        navMeshSurface.navMeshData = levelDataSO.navMeshData;   
        navMeshSurface.AddData();
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
        Debug.Log("STAGE " + stageNumber + " DONT EXIST IN STAGE MANAGER");
        return null;
    }
}
