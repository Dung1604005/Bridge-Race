using System.Collections.Generic;
using UnityEngine;

public static class ColliderCache<T> where T: MonoBehaviour
{
    private static Dictionary<Collider , T> colliderDict = new Dictionary<Collider, T>();

    public static void AddComponent(Collider collider, T component)
    {
        if(collider == null || component == null)return;
        if (colliderDict.ContainsKey(collider))
        {
            colliderDict[collider] = component;
        }
        else
        {
            colliderDict.Add(collider, component);
        }
    }

    public static void RemoveComponent(Collider collider)
    {
        if(collider == null) return;

        if (colliderDict.ContainsKey(collider))
        {
            colliderDict.Remove(collider);
        }
    }

    public static void ClearAll()
    {
        colliderDict.Clear();
    }

    public static T GetComponent(Collider collider)
    {
        if(collider == null || !colliderDict.ContainsKey(collider))
        {
            T component = collider.GetComponent<T>();

            AddComponent(collider, component);
        }

        return colliderDict[collider];
    }
}