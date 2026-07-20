using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataBase", menuName = "Scriptable Objects/LevelDataBase")]
public class LevelDataBase : ScriptableObject
{
    [SerializeField] private List<LevelDataSO> levelDataBase = new List<LevelDataSO>();

    public LevelDataSO GetLevelData(int levelId)
    {
        if(levelId < 0 || levelId >= levelDataBase.Count)
        {
            return null;
        }

        return levelDataBase[levelId];
    }
}
