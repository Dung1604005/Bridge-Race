using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offSetPlaying;

    [SerializeField] private Vector3 offSetOnWin;

    [SerializeField] private Camera cam;


    [SerializeField] private Transform tfPlayer;

    [SerializeField] private Transform tfWin;

    [SerializeField] private float speed ;

    [SerializeField] private float fieldOfView;


    [SerializeField]private Transform tf;

    [SerializeField]private Transform target;

    private Vector3 offSet = Vector3.zero;

    private bool usingLerp = false;

   
    public void OnWin()
    {
        target = tfWin;
        offSet = offSetOnWin;
        usingLerp = true;
        tf.rotation = Quaternion.Euler(new Vector3(10f, 0f, 0f));
    
    }

    public void OnInit()
    {
        usingLerp = false;
        offSet = offSetPlaying;
        target = tfPlayer;
        cam.fieldOfView = 60f;
        tf.rotation = Quaternion.Euler(new Vector3(50f, 0f, 0f));


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
        
    }


    void LateUpdate()
    {
        if(target == null)
        {
            return;
        }
        //TODO: Cho cam bay dan den win pos khi win
        if (usingLerp)
        {
            tf.position = Vector3.Lerp(tf.position, offSet + target.position, 5f*Time.deltaTime);
        }
        else
        {
            tf.position = offSet + target.position;
        }
    }
}
