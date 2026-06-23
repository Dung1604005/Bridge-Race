using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private float rotationSpeed;
    private float moveX;

    private float moveZ;

    private Quaternion targetRotation;

    public void GetInputMove()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
    }

    public void ChangeRotation()
    {
        if(tf == null)
        {
            Debug.LogError("PLAYER HAVENT SET TRANSFORM");
        }

        targetRotation = Quaternion.LookRotation(new Vector3(moveX, 0f, moveZ));
        tf.rotation= Quaternion.Slerp(tf.rotation, targetRotation, rotationSpeed*Time.fixedDeltaTime);
        
    }
    public void Move()
    {
        rb.linearVelocity = new Vector3(moveX,0f, moveZ).normalized*speed;

        if((new Vector3(moveX, 0f, moveZ).sqrMagnitude <= 0.05)){
            ChangeAnim(GameData.Instance.ANIM_IDLE);
        }
        else
        {
            ChangeAnim(GameData.Instance.ANIM_RUN);
            ChangeRotation();
        }
        
    }
    void Awake()
    {
        tf = this.transform;
    }
    void FixedUpdate()
    {
        Move();
    }
    void Update()
    {
        GetInputMove();
    }
}
