using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;

    [SerializeField] private Camera cam;


    [SerializeField] private Transform tfPlayer;

    [SerializeField] private Transform tfWin;

    [SerializeField] private float speed ;

    [SerializeField] private float fieldOfView;

    private Transform tf;

    private Transform target;

   
    public void OnWin()
    {
        target = tfWin;
        offSet = Vector3.zero + new Vector3(0, 5f, 0f);
        tf.rotation = Quaternion.Euler(new Vector3(10f, 0f, 0f));
        cam.fieldOfView = 0f;

        StartCoroutine(IEZoomOut(0.5f, fieldOfView));


    }

    IEnumerator IEZoomOut(float duration, float fieldView)
    {
        float timer = 0f;
        float startFieldView = 0f;
        while(timer + 0.01f < duration)
        {
            timer += Time.deltaTime;

            cam.fieldOfView = Mathf.Lerp(startFieldView, fieldView, timer/duration );
            yield return null;
        }

    }

    void Awake()
    {
        tf = this.transform;
        target = tfPlayer;
    }


    void LateUpdate()
    {
        tf.position = offSet + target.position;
    }
}
