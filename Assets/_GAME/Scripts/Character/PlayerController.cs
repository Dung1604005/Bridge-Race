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
        inputActions.Player.Movement.performed += GetInputMove;
        inputActions.Player.Movement.canceled += EndInputMove;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        inputActions.Disable();
        inputActions.Player.Movement.performed -= GetInputMove;
        inputActions.Player.Movement.canceled -= EndInputMove;
    }

    public void GetInputMove(InputAction.CallbackContext context)
    {
        if(IsInActive)return;
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

        if (blockMoveDown)
        {
            if (moveZ < -0.01f)
            {
                moveZ = 0f;
            }
        }

        if (blockMoveForward)
        {
            if (moveZ > 0.01f)
            {
                moveZ = 0f;
            }

        }
        tf.position = Vector3.MoveTowards(tf.position, tf.position + new Vector3(moveX, 0f, moveZ).normalized, speed*Time.deltaTime);

        if ((new Vector3(moveX, 0f, moveZ).sqrMagnitude <= 0.05))
        {
            ChangeAnim(GameData.Instance.ANIM_IDLE);
        }
        else
        {
            ChangeAnim(GameData.Instance.ANIM_RUN);
            ChangeRotation();
        }

    }

    protected override void Awake()
    {
        inputActions = new PlayerInputAction();
        base.Awake();
    }

    void FixedUpdate()
    {
        if (IsInActive)
        {
            return;
        }
        CheckForward();
        Move();

    }
    protected override void Update()
    {
        if (!IsInActive)
        {

            if (CharacterIsFalling())
            {
                EventBus<OnCharacterInActive>.Raise(new OnCharacterInActive
                {
                    CharacterId = CharacterId
                });
                OnDespawn();
                Invoke(nameof(ReSpawn), 0.5f);
                return;
            }
            base.Update();

        }

    }
}
