using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Character : MonoBehaviour
{
    //Thong so

    [Header("THONG SO")]

    [SerializeField] protected int characterId;
    [SerializeField] protected float speed;

    [SerializeField] protected float rangeDetect;

    [SerializeField] private ColorType colorType;

    [SerializeField] private float knockForce;
    

    [Header("REFERENCE")]

    [SerializeField] protected Stage currentStage;

    [SerializeField] protected Animator anim;

    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected Stack<Brick> characterBricks = new Stack<Brick>();

    [SerializeField] private int visualBrickId = 0;

    [SerializeField] private Renderer renderer;

    [SerializeField] private Vector3 startCharacterBrickPos;

    private int layerGround;

    private int layerStair;

    private int layerGate;



    protected Transform tf;

    private String currentAnim = "";

    protected bool blockMoveForward;

    protected bool blockMoveDown;

    [SerializeField]protected bool canMove;


    private bool isDead;

    public bool IsDead => isDead;

    public Transform TF => tf;
    public ColorType ColorType => colorType;

    public Stage CurrentStage => currentStage;

    public bool BlockMoveForward => blockMoveForward;

    public bool BlockMoveDown => blockMoveDown;

    public int CharacterId => characterId;


    public virtual void OnEnable()
    {
        EventBus<OnWin>.Subcribe(OnWin);
    }
    public virtual void OnDisable()
    {
        EventBus<OnWin>.UnSubcribe(OnWin);
    }
    public virtual void OnWin(OnWin onWin)
    {
        BlockMove();
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        tf.rotation = targetRotation;
        ClearBrick();
        ChangeAnim(GameData.Instance.ANIM_IDLE);

    }
    public void SetSpawn(Vector3 pos)
    {
        tf.position = pos;
    }
    public virtual void ReSpawn()
    {
        SetSpawn(currentStage.GetSpawnPosCharacter(this));
        OnInit();
    }

    public virtual void OnInit()
    {

        blockMoveForward = false;
        canMove = true;
        isDead = false;
        ClearBrick();
        visualBrickId = 0;
    }
    public virtual void OnDespawn()
    {
        blockMoveForward = false;
        isDead = true;
    }

    public virtual void ChangeStage(Stage newStage)
    {
        if (newStage == null)
        {
            Debug.Log("New stage dont exist");
            return;
        }
        if (newStage.StageNumber <= currentStage.StageNumber)
        {
            return;
        }
        currentStage.RemoveCharacter(this);
        newStage.AddCharacter(this);
        currentStage = newStage;

        EventBus<OnCharacterUpStage>.Raise(new OnCharacterUpStage
        {
            Character = this,
            Stage = currentStage.StageNumber
        });
    }

    public void SetColor(ColorType colorType)
    {
        if (renderer != null)
        {
            renderer.material = GameData.Instance.ColorDataSO.GetColorCharacterMaterial(colorType);
        }
        else
        {
            Debug.LogError("DONT HAVE COLOR MATERIAL");
        }
    }

    public void ChangeAnim(String newAnim)
    {
        if (!string.IsNullOrEmpty(currentAnim))
        {
            anim.ResetTrigger(currentAnim);
        }
        anim.SetTrigger(newAnim);
        currentAnim = newAnim;

    }

    public void BlockMove()
    {
        canMove = false;
        rb.linearVelocity = Vector3.zero;
    }

    public virtual bool CharacterIsGoingDown()
    {

        if (rb.linearVelocity.z < -0.01f)
        {
            return true;
        }
        return false;
    }

    public virtual bool CharacterIsFalling()
    {
        if (rb.linearVelocity.y < -5f && !Physics.Raycast(tf.position, -tf.up, 5f, layerGround))
        {
            return true;
        }
        return false;
    }

    public void CheckForward()
    {


        //Check Gate
        if (Physics.Raycast(tf.position, tf.forward, out RaycastHit hitGate, rangeDetect, layerGate))
        {
            Collider col = hitGate.collider;


            GateCtrl gate = ColliderCache<GateCtrl>.GetComponent(col);



            if (gate.NextStage == currentStage || gate.NextStage == null)
            {
                blockMoveDown = true;
            }
            else
            {
                blockMoveDown = false;
            }
        }
        else
        {
            blockMoveDown = false;
        }
        //Check stair
        if (CharacterIsGoingDown())
        {
            return;
        }
        if (Physics.Raycast(tf.position, tf.forward, out RaycastHit hit, rangeDetect, layerStair))
        {

            Collider col = hit.collider;


            Stair stair = ColliderCache<Stair>.GetComponent(col);


            stair.TakeStair(this);
            if (stair.ColorType == colorType)
            {
                blockMoveForward = false;
            }
            else
            {
                blockMoveForward = true;
            }
        }
        else
        {
            blockMoveForward = false;
        }

    }

    public int GetNextBrickIndex()
    {
        int assignedIndex = visualBrickId;
        visualBrickId += 1;
        return assignedIndex;
    }
    public Vector3 GetNextBrickPosition(int index)
    {

        return startCharacterBrickPos + new Vector3(0f, index * (GameData.Instance.BRICK_SIZE.y + 0.05f), 0f) + tf.position;
    }
    public int GetAmountBrick()
    {
        return visualBrickId;
    }

    public void AddBrick()
    {
        Vector3 localPos = startCharacterBrickPos + new Vector3(0f, characterBricks.Count * (GameData.Instance.BRICK_SIZE.y + 0.08f), 0f);
        Brick brick = SimplePool.Spawn<Brick>(PoolType.BrickPool, Vector3.zero, Quaternion.identity);

        brick.OnInit();
        brick.SetLocal(localPos, Quaternion.identity, tf);
        brick.SetColor(colorType);
        brick.SetActiveTrail(false);

        characterBricks.Push(brick);
        

        BrickEffect brickEffect = SimplePool.Spawn<BrickEffect>(PoolType.BrickEffectPool, Vector3.zero, Quaternion.identity);
        brickEffect.SetColor(colorType);
        brickEffect.SetLocal(localPos, Quaternion.identity, tf);
        brickEffect.Play();
    }

    public void RemoveBrick()
    {
        if (characterBricks.Count == 0)
        {
            Debug.Log("BRICK IS EMPTY BUT TRY TO POP");
            return;
        }
        Brick brick = characterBricks.Pop();
        currentStage.ReSpawnBrick(brick.ColorType);
        visualBrickId -= 1;
        brick.OnDespawn();
    }

    public  void ClearBrick()
    {
        
        while (characterBricks.Count > 0)
        {
            RemoveBrick();
        }
        
    }

    public virtual void Knockback()
    {
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
        BlockMove();
        Vector3 knockbackDirection = -tf.forward;
        rb.AddForce(knockbackDirection * knockForce, ForceMode.Impulse);
        ClearBrick();
        ChangeAnim(GameData.Instance.ANIM_KNOCKBACK);

        
        Invoke(nameof(StandUp), 2f);
        
    }

    public virtual void StandUp()
    {
        isDead = false;
        canMove = true;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    

    public void OnCollisionEnter(Collision collision)
    {
        if (!canMove)
        {
            return;
        }
        
        Collider collider = collision.collider;
        if (collider.CompareTag(GameData.Instance.CHARACTER_TAG))
        {
            
            Character character = ColliderCache<Character>.GetComponent(collider);

            if(!character.canMove)return;
            if(character.GetAmountBrick() < GetAmountBrick())
            {
                character.Knockback();
            }
            else if(character.GetAmountBrick() > GetAmountBrick())
            {
                Knockback();
            }
            else
            {
                Knockback();
                character.Knockback();
            }

        }
    }

    protected virtual void Awake()
    {
        layerGround = LayerMask.GetMask("Ground", "Stair");
        layerStair = LayerMask.GetMask("Stair");
        layerGate = LayerMask.GetMask("Gate");
        anim.applyRootMotion = false;
        tf = this.transform;
        SetColor(colorType);
        OnInit();

    }

    protected virtual void Update()
    {
        foreach (Brick brick in characterBricks)
        {

            brick.Shake();
        }

        if (!canMove)
        {
            return;
        }
    }
}
