using UnityEngine;

public class UIFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform tf;

    [SerializeField] private Transform target;

    void LateUpdate()
    {
        tf.position = offset + target.position;
    }
}
