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

        targetRotation = Quaternion.LookRotation(new Vector3(moveX, 0f, moveZ).normalized);
        tf.rotation= Quaternion.Slerp(tf.rotation, targetRotation, rotationSpeed*Time.fixedDeltaTime);
        
    }
    public void Move()
    {
        float moveY = rb.linearVelocity.y;
        if(moveY > 0.01f)
        {
            moveY = 0f;
        }

        if (blockMoveForward)
        {
            if(moveZ > 0.01f)
            {
                moveZ = 0f;
            }
            
        }
        rb.linearVelocity = new Vector3(moveX,moveY, moveZ).normalized*speed;
        
        if((new Vector3(moveX, 0f, moveZ).sqrMagnitude <= 0.05)){
            ChangeAnim(GameData.Instance.ANIM_IDLE);
        }
        else
        {
            ChangeAnim(GameData.Instance.ANIM_RUN);
            ChangeRotation();
        }
        
    }
    
    void FixedUpdate()
    {
        CheckStairForward();
        Move();
        
    }
    protected override void Update()
    {
        base.Update();
        GetInputMove();
    }
}
