using UnityEngine;

public class PoolController : MonoBehaviour
{
    [SerializeField] PoolAmount[] poolAmounts;

    void Awake()
    {
        for(int i = 0; i < poolAmounts.Length; i++)
        {
            SimplePool.PreLoad(poolAmounts[i].prefab, poolAmounts[i].amount, poolAmounts[i].parent);
        }
    }
}

[System.Serializable]

public class PoolAmount
{
    public GameUnit prefab;

    public Transform parent;

    public int amount;
}
