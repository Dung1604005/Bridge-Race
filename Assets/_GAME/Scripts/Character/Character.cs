using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Character : MonoBehaviour
{
    //Thong so

    [SerializeField] protected int characterId;
    [SerializeField] protected float speed;

    [SerializeField] protected float rangeDetect;

    [SerializeField] private ColorType colorType;

    [SerializeField] private Renderer renderer;

    [SerializeField] protected Animator anim;

    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected Stack<Brick> characterBricks = new Stack<Brick>();

    [SerializeField] protected Stage currentStage;

    [SerializeField] private Vector3 startCharacterBrickPos;

    [SerializeField]private int visualBrickId = 0;

    

    protected Transform tf;

    private String currentAnim;

    protected bool blockMoveForward;

    protected bool canMove;


    private bool isDead;

    public bool IsDead => isDead;

    public Transform TF => tf;
    public ColorType ColorType => colorType;

    public Stage CurrentStage => currentStage;

    public bool BlockMoveForward => blockMoveForward;

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
        Quaternion targetRotation = Quaternion.LookRotation(-tf.forward);
        tf.rotation = targetRotation;
        ClearBrick();
        ChangeAnim(GameData.Instance.ANIM_IDLE);
        
    }
    public void SetSpawn(Vector3 pos)
    {
        tf.position = pos;
    }
    public void ReSpawn()
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

    public void ChangeStage(Stage newStage)
    {
        if(newStage == null)
        {
            Debug.Log("New stage dont exist");
            return;
        }
        if(newStage.StageNumber <= currentStage.StageNumber)
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
        if(renderer != null)
        {
            renderer.material = GameData.Instance.ColorDataSO.GetColorCharacterMaterial(colorType) ;
        }
        else
        {
            Debug.LogError("DONT HAVE COLOR MATERIAL");
        }
    }

    public void ChangeAnim(String newAnim)
    {
        
        anim.SetTrigger(newAnim);
        currentAnim = newAnim;
        
    }

    public void BlockMove()
    {
        canMove = true;
    }

    public virtual bool CharacterIsGoingDown()
    {
        
        if(rb.linearVelocity.z < -0.01f)
        {
            return true;
        }
        return false;
    }

    public virtual bool CharacterIsFalling()
    {
        if(rb.linearVelocity.y < -5f && !Physics.Raycast(tf.position, -tf.up,3f, 1<<7 ))
        {
            return true;
        }
        return false;
    }

    public void CheckStairForward()
    {
       
        if (CharacterIsGoingDown())
        {
            return;
        }
      
        if(Physics.Raycast(tf.position, tf.forward, out RaycastHit hit,rangeDetect,1<<6))
        {
            
            Collider col = hit.collider;

            
            Stair stair = ColliderCache<Stair>.GetComponent(col);


            stair.TakeStair(this);
            if(stair.ColorType == colorType)
            {
                if (stair.IsThisLastStair())
                {
                    ChangeStage(stair.Bridge.NextStage);
                }
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
        
       return startCharacterBrickPos + new Vector3(0f, index*(GameData.Instance.BRICK_SIZE.y + 0.05f), 0f) + tf.position;
    }
    public int GetAmountBrick()
    {
        return visualBrickId ;
    }

    public void AddBrick()
    {
        Vector3 localPos =startCharacterBrickPos + new Vector3(0f, characterBricks.Count*(GameData.Instance.BRICK_SIZE.y + 0.08f), 0f);
        Brick brick = SimplePool.Spawn<Brick>(PoolType.BrickPool,Vector3.zero, Quaternion.identity );
        
        brick.transform.SetParent(tf, true);
        brick.OnInit();
        brick.TF.localPosition = localPos;
        brick.TF.localRotation = Quaternion.identity;
        brick.SetColor(colorType);
        characterBricks.Push(brick);
        brick.SetActiveTrail(false);

        BrickEffect brickEffect = SimplePool.Spawn<BrickEffect>(PoolType.BrickEffectPool,Vector3.zero, Quaternion.identity );
        brickEffect.SetColor(colorType);
        brickEffect.transform.SetParent(tf, true);
        brickEffect.TF.localPosition = localPos;
        brickEffect.TF.localRotation = Quaternion.identity;
        brickEffect.Play();
                
    }

    public void RemoveBrick()
    {
        Brick brick = characterBricks.Pop();
        currentStage.ReSpawnBrick(brick.ColorType);
        visualBrickId -= 1;
        brick.OnDespawn();
    }

    public void ClearBrick()
    {
        while(characterBricks.Count > 0)
        {
            RemoveBrick();
        }
    }

    void Awake()
    {
        anim.applyRootMotion = false;
        tf = this.transform;
        SetColor(colorType);
        OnInit();
        
    }

    protected virtual void Update()
    {
        foreach(Brick brick in characterBricks)
        {
            
            brick.Shake();
        }

        if (!canMove)
        {
            return;
        }
    }
}
