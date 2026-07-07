using UnityEngine;

public class GameUnit : MonoBehaviour
{
    public PoolType PoolType;

    [SerializeField]protected Transform tf;

    public Transform TF => tf;
}


public enum PoolType{
    BrickPool = 0,
    BrickEffectPool = 1,

    BridgePool = 2,

    StairPool = 3,
    StagePool = 4,

    GatePool = 5
}