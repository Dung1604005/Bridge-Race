using UnityEngine;

public class GameUnit : MonoBehaviour
{
    public PoolType PoolType;

    [SerializeField]protected Transform tf;

    public Transform TF => tf;
}


public enum PoolType{
    BrickPool = 0,
    BrickEffectPool = 1
}