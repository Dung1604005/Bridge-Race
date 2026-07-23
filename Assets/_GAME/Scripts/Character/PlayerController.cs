using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    [SerializeField] private float rotationSpeed;
    private float moveX;

    private float moveZ;

    private PlayerInputAction inputActions;

    private Quaternion targetRotation;

    public override void OnEnable()
    {
        base.OnEnable();
        inputActions.Enable();
        inputActions.Player.Movement.performed += OnMove;
        inputActions.Player.Movement.canceled += EndInputMove;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        inputActions.Disable();
        inputActions.Player.Movement.performed -= OnMove;
        inputActions.Player.Movement.canceled -= EndInputMove;
    }
    public override bool CharacterIsGoingDown()
    {

        if (moveZ < -0.01f)
        {
            return true;
        }
        return false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        

        moveX = context.ReadValue<Vector2>().x;
        moveZ = context.ReadValue<Vector2>().y;
    }

    public void EndInputMove(InputAction.CallbackContext context)
    {
        moveX = 0f;
        moveZ = 0f;
    }

    public void ChangeRotation()
    {
        if (tf == null)
        {
            Debug.LogError("PLAYER HAVENT SET TRANSFORM");
        }

        targetRotation = Quaternion.LookRotation(new Vector3(moveX, 0f, moveZ).normalized);
        tf.rotation = Quaternion.Slerp(tf.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

    }
    public void Move()
    {
        
        // float moveY = rb.linearVelocity.y;
        // if (moveY > 0.01f)
        // {
        //     moveY = 0f;
        // }

        
        if (new Vector3(moveX, 0f, moveZ).sqrMagnitude <= 0.00001)
        {
            ChangeAnim(GameData.Instance.ANIM_IDLE);
        }
        else
        {
            ChangeAnim(GameData.Instance.ANIM_RUN);
            ChangeRotation();
        }
        if (characterState.BlockDown)
        {
            if (moveZ < -0.01f)
            {
                moveZ = 0f;
            }
        }

        if (characterState.BlockForward)
        {
            if (moveZ > 0.01f)
            {
                moveZ = 0f;
            }

        }
        Vector3 dir = new Vector3(moveX, 0f, moveZ);
        if(dir.sqrMagnitude > 1)
        {
           dir = dir.normalized; 
        }


        tf.position = Vector3.MoveTowards(tf.position, tf.position + dir, speed*Time.fixedDeltaTime);

    }

    protected override void Awake()
    {
        inputActions = new PlayerInputAction();
        base.Awake();
    }

    void FixedUpdate()
    {
         if(GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }
        if (characterState.IsInactive)
        {
            return;
        }
        CheckForward();
        Move();

    }
    protected override void Update()
    {
        if (!characterState.IsInactive)
        {

            if (CharacterIsFalling())
            {
                EventBus<OnCharacterInActive>.Raise(new OnCharacterInActive
                {
                    CharacterId = CharacterId
                });
                //OnDespawn();
                Invoke(nameof(ReSpawn), 0.5f);
                return;
            }
            base.Update();

        }

    }
}
