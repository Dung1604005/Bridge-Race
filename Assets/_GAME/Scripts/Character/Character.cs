using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Character : MonoBehaviour
{
    //Thong so

    [Header("THONG SO")]

    [SerializeField] protected int characterId;

    [SerializeField] protected string characterName;
    [SerializeField] protected float speed;

    [SerializeField] protected float rangeDetect;

    [SerializeField] private ColorType colorType;

    [SerializeField] private float knockForce;

    public bool IsBot;


    [Header("REFERENCE")]

    [SerializeField] protected ParticleSystem breakBrickEffect;

    [SerializeField] protected Stage currentStage;

    [SerializeField] protected Animator anim;

    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected List<Brick> characterBricks = new List<Brick>();

    [SerializeField] private int visualBrickId = 0;

    [SerializeField] private Renderer renderer;

    [SerializeField] private Vector3 startCharacterBrickPos;

    [SerializeField] protected Transform tf;

    [SerializeField] private bool isInActive;

    [SerializeField] private bool isOnGround;

    private int layerGround;

    private int layerStair;

    private int layerGate;

    private String currentAnim = "";

    protected bool blockMoveForward;

    protected bool blockMoveDown;

    public bool IsInActive => isInActive;


    public Transform TF => tf;
    public ColorType ColorType => colorType;

    public Stage CurrentStage => currentStage;

    public int CharacterId => characterId;

    public String CharacterName => characterName;
    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {

    }
    public virtual void OnWin()
    {
        SetInActive();
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
    public virtual void OnStart()
    {
        rb.useGravity = true;
    }

    public virtual void OnInit()
    {
        SetColor(colorType);
        blockMoveForward = false;
        isInActive = false;
        ClearBrick();
        visualBrickId = 0;
    }
    public virtual void OnDespawn()
    {
        blockMoveForward = false;
        rb.useGravity = false;
        SetInActive();
    }

    public virtual void ChangeStage(Stage newStage, bool raiseEvent = true)
    {
        if (newStage == null)
        {
            Debug.Log("New stage dont exist");
            return;
        }
        if (currentStage != null && newStage.StageNumber <= currentStage.StageNumber)
        {
            return;
        }
        if (currentStage != null)
        {

            currentStage.RemoveCharacter(this);
        }
        newStage.AddCharacter(this);
        currentStage = newStage;
        if (raiseEvent)
        {
            EventBus<OnCharacterUpStage>.Raise(new OnCharacterUpStage
            {
                Character = this,
                Stage = currentStage.StageNumber
            });
        }


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

    public void SetInActive(float duration = 0f)
    {
        isInActive = true;
        rb.linearVelocity = Vector3.zero;

        if (duration > 0.01f)
        {
            StartCoroutine(IESetActive(duration));
        }
    }

    IEnumerator IESetActive(float duration)
    {
        yield return new WaitForSeconds(duration);

        isInActive = false;

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

        if (rb.linearVelocity.y < -5f && !Physics.Raycast(tf.position, -tf.up, 10f, layerGround))
        {
            isOnGround = false;
            return true;
        }
        isOnGround = true;
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

    public void RemoveBrickIndex()
    {

        visualBrickId = Math.Max(0, visualBrickId - 1);
    }
    public Vector3 GetBrickPosition(int index)
    {

        return startCharacterBrickPos + new Vector3(0f, index * (GameData.Instance.BRICK_SIZE.y / 2 + 0.05f), 0f) + tf.position;
    }
    public int GetAmountBrick()
    {
        return visualBrickId;
    }

    public void AddBrick()
    {
        Vector3 localPos = startCharacterBrickPos + new Vector3(0f, characterBricks.Count * (GameData.Instance.BRICK_SIZE.y / 2 + 0.05f), 0f);
        Brick brick = SimplePool.Spawn<Brick>(PoolType.BrickPool, Vector3.zero, Quaternion.identity);

        brick.OnInit();

        brick.SetLocal(localPos, Quaternion.identity, tf);
        brick.SetColor(colorType);
        brick.SetActiveTrail(false);

        characterBricks.Add(brick);


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
        Brick brick = characterBricks[characterBricks.Count - 1];
        characterBricks.RemoveAt(characterBricks.Count - 1);
        currentStage.ReSpawnBrick(brick.ColorType);
        visualBrickId -= 1;
        brick.OnDespawn();
    }

    public void ClearBrick()
    {

        while (characterBricks.Count > 0)
        {
            RemoveBrick();
        }
        visualBrickId = 0;

    }

    public virtual void Knockback(Vector3 knockbackDirection)
    {

        SetInActive();
        //gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
        rb.AddForce(knockbackDirection * knockForce, ForceMode.Impulse);
        tf.rotation = Quaternion.LookRotation(-knockbackDirection);
        if (characterBricks.Count > 0)
        {
            breakBrickEffect.transform.position = GetBrickPosition(visualBrickId / 2);
            breakBrickEffect.Play();
        }
        ClearBrick();
        ChangeAnim(GameData.Instance.ANIM_KNOCKBACK);

        EventBus<OnCharacterInActive>.Raise(new OnCharacterInActive
        {
            CharacterId = CharacterId
        });


        Invoke(nameof(StandUp), 2f);

    }

    public virtual void StandUp()
    {
        isInActive = false;
        //gameObject.layer = LayerMask.NameToLayer("Player");
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (IsInActive)
        {
            return;
        }

        Collider collider = collision.collider;
        if (collider.CompareTag(GameData.Instance.CHARACTER_TAG))
        {

            Character character = ColliderCache<Character>.GetComponent(collider);

            if (character.IsInActive) return;
            Vector3 knockbackDirBA = character.tf.position - tf.position;
            Vector3 knockbackDirAB = tf.position - character.tf.position;
            knockbackDirAB.y = 0.8f;
            knockbackDirBA.y = 0.8f;
            knockbackDirAB.Normalize();
            knockbackDirBA.Normalize();
            if (character.GetAmountBrick() < GetAmountBrick())
            {
                character.Knockback(knockbackDirBA);
                SetInActive(0.1f);


            }
            else if (character.GetAmountBrick() > GetAmountBrick())
            {
                Knockback(knockbackDirAB);
                character.SetInActive(0.1f);

            }
            else
            {
                Knockback(knockbackDirAB);
                character.Knockback(knockbackDirBA);
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
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }
        foreach (Brick brick in characterBricks)
        {

            brick.Shake();
        }

        if (IsInActive)
        {
            return;
        }
    }
}
