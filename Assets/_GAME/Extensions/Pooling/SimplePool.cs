using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public static class SimplePool
{
    private static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();

    //Khoi tao pool moi
    public static void PreLoad(GameUnit prefab, int amount, Transform parent)
    {
        if(prefab == null)
        {
            Debug.LogError("PREFAB IS EMPTY");
            return;
        }

        if (!poolInstance.ContainsKey(prefab.PoolType) || poolInstance[prefab.PoolType] == null)
        {
            Pool pool = new Pool();
            pool.PreLoad(prefab, amount, parent);
            poolInstance[prefab.PoolType] = pool;
        }
        
    }

    //lay phan tu

    public static T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T: GameUnit
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + " IS NOT PRELOAD");
            return null;
        }
        return poolInstance[poolType].Spawn(pos, rot) as T;
    }

    //Tra phan tu

    public static void Despawn(GameUnit gameUnit)
    {
        if (!poolInstance.ContainsKey(gameUnit.PoolType))
        {
            Debug.LogError(gameUnit.PoolType + " IS NOT PRELOAD");
            return ;
        }
        poolInstance[gameUnit.PoolType].Despawn(gameUnit);
    }

    //Thu thap phan tu
    public static void Collect(PoolType poolType)
    {
         if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + " IS NOT PRELOAD");
            return ;
        }
        poolInstance[poolType].Collect();
    }

    public static void CollectAll()
    {
        foreach(Pool pool in poolInstance.Values)
        {
            pool.Collect();
        }
    }

    //Destroy 1 pool

    public static void Release(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + " IS NOT PRELOAD");
            return ;
        }
        poolInstance[poolType].Release();
    }

    public static void ReleaseAll()
    {
          foreach(Pool pool in poolInstance.Values)
        {
            pool.Release();
        }
    }
}

public class Pool
{
    Transform parent;

    GameUnit prefab;

    //list unit chua duoc su dung
    Queue<GameUnit> inactives = new Queue<GameUnit>();

    //list unit dang duoc su dung
    List<GameUnit> actives = new List<GameUnit>();

    //Khoi tao pool

    public void PreLoad(GameUnit prefab, int amount, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        for(int i = 0; i < amount; i++)
        {
            Despawn(Spawn(Vector3.zero, Quaternion.identity));
        }
    }

    //Lay phan tu tu pool

    public GameUnit Spawn(Vector3 pos, Quaternion rot)
    {
        GameUnit unit;

        if(inactives.Count <= 0)
        {
            unit = GameObject.Instantiate(prefab, parent);
        }
        else
        {
            unit = inactives.Dequeue();
        }

        unit.TF.SetPositionAndRotation(pos, rot);
        actives.Add(unit);
        unit.gameObject.SetActive(true);
        return unit;
    }

    //Tra phan tu ve pool

    public void Despawn(GameUnit gameUnit)
    {
        if (gameUnit != null && gameUnit.gameObject.activeSelf)
        {
            actives.Remove(gameUnit);
            inactives.Enqueue(gameUnit);
            gameUnit.gameObject.SetActive(false);
            
        }
    }

    //Thu thap tat ca phan tu dang dung ve pool

    public void Collect()
    {
        while(actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }

    //Destroy toan bo unit

    public void Release()
    {
        Collect();
        while(inactives.Count > 0)
        {
            GameObject.Destroy(inactives.Dequeue().gameObject);

        }
        inactives.Clear();
    }
}