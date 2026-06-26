using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;


    [SerializeField] private Transform tfPlayer;

    [SerializeField] private float speed ;

    private Transform tf;

    void Awake()
    {
        tf = this.transform;
    }


    void LateUpdate()
    {
        tf.position = offSet + tfPlayer.position;
    }
}
