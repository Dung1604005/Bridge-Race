using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Character : MonoBehaviour
{
    //Thong so
    [SerializeField] protected float speed;

    [SerializeField] protected float rangeDetect;

    [SerializeField] private ColorType colorType;

    [SerializeField] private Renderer renderer;

    [SerializeField] protected Animator anim;

    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected Queue<Brick> characterBricks = new Queue<Brick>();

    [SerializeField] protected Stage currentStage;

    [SerializeField] private Vector3 startCharacterBrickPos;

    [SerializeField]private int visualBrickId = 0;

    

    protected Transform tf;

    private String currentAnim;

    private bool blockMoveForward;

    private bool isDead;

    public bool IsDead => isDead;

    public Transform TF => tf;
    public ColorType ColorType => colorType;

    public Stage CurrentStage => currentStage;

    public bool BlockMoveForward => blockMoveForward;

    
    public void OnInit()
    {
        
        blockMoveForward = false;
        anim.applyRootMotion = false;
        isDead = false;
    }
    public void OnDespawn()
    {
        blockMoveForward = false;
        isDead = true;
    }

    public void SetColor(ColorType colorType)
    {
        if(renderer != null)
        {
            renderer.material = GameData.Instance.ColorDataSO.GetColorMaterial(colorType) ;
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

    public void CheckStairForward()
    {
        Debug.DrawRay(tf.position, tf.forward * rangeDetect, Color.red);
        if(Physics.Raycast(tf.position, tf.forward, out RaycastHit hit,rangeDetect,1<<6))
        {
            Collider col = hit.collider;

            
            Stair stair = ColliderCache<Stair>.GetComponent(col);
            if(stair == null)
            {
                stair = col.gameObject.GetComponent<Stair>();
                ColliderCache<Stair>.AddComponent(col, stair);
            }
            
            if(stair.ColorType == colorType)
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
            Debug.Log("Ray cast miss");
            blockMoveForward = false;
        }

    }

    public int GetBrickIndex()
    {
        int assignedIndex = visualBrickId;
        visualBrickId += 1;
        return assignedIndex;
    }
    public Vector3 GetNextBrickPosition(int index)
    {
        
       return startCharacterBrickPos + new Vector3(0f, index*(GameData.Instance.BRICK_SIZE.y + 0.08f), 0f) + tf.position;
    }
    public int GetAmountBrick()
    {
        return characterBricks.Count;
    }
    public void AddBrick()
    {
        Vector3 localPos =startCharacterBrickPos + new Vector3(0f, characterBricks.Count*(GameData.Instance.BRICK_SIZE.y + 0.08f), 0f);
        Brick brick = SimplePool.Spawn<Brick>(PoolType.BrickPool,Vector3.zero, Quaternion.identity );
        
        brick.transform.SetParent(tf, true);
        brick.TF.localPosition = localPos;
        brick.TF.localRotation = Quaternion.identity;
        brick.OnInit(null, brick.TF.position);
        brick.SetColor(colorType);
        characterBricks.Enqueue(brick);
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
        Brick brick = characterBricks.Dequeue();
        brick.OnDespawn();
        SimplePool.Despawn(brick);

    }

    void Awake()
    {
        tf = this.transform;
        SetColor(colorType);
        
    }
}
