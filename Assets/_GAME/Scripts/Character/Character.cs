using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] protected TextMeshProUGUI textName;

    [SerializeField] protected ParticleSystem breakBrickEffect;

    [SerializeField] protected Stage currentStage;

    [SerializeField] protected Animator anim;

    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected BrickCharacterManager brickCharacterManager;

    [SerializeField] private Renderer renderer;

    [SerializeField] private Collider collider;

    [SerializeField] protected Transform tf;

    [SerializeField] protected Transform tfVisual;

    [SerializeField] protected CharacterState characterState;

    [SerializeField] private SkinController skinPrefab;

    private int layerGround;

    private int layerStair;

    private LayerMask layerStairGround;

    private int layerGate;

    private String currentAnim = "";

    public BrickCharacterManager BrickCharacterManager => brickCharacterManager;


    public Transform TF => tf;
    public ColorType ColorType => colorType;

    public Stage CurrentStage => currentStage;

    public int CharacterId => characterId;

    public String CharacterName => characterName;

    public CharacterState CharacterState => characterState;
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
        brickCharacterManager.ClearBrick();
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
        characterState = new CharacterState();
        brickCharacterManager.ClearBrick(); 
    
    }
    public virtual void OnDespawn()
    {
        
        SetColor(ColorType.NONE);
        characterState = new CharacterState();
        rb.useGravity = false;
        SetInActive();
        brickCharacterManager.ClearBrick();
        currentStage = null;
        
    }

    public virtual void OnPause()
    {
        rb.useGravity = false;
        SetInActive();
    }

    public virtual void OnContinue()
    {
        rb.useGravity = true;
        characterState.IsInactive = false;
    }

    public void SetName(String characterName)
    {
        this.characterName = characterName;
        textName.text = characterName;
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

    public void SetSkin(SkinController skinController)
    {
        if(skinPrefab != null && skinPrefab.gameObject != null)
        {
            Destroy(skinPrefab.gameObject);
        }
        
        skinPrefab = Instantiate(skinController, tfVisual);
        renderer = skinPrefab.SkinRenderer;
        anim = skinPrefab.Anim;
        brickCharacterManager.SetBrickRoot(skinPrefab.BrickRoot);


    }

    public virtual void SetSpeed(float speed)
    {
        this.speed = speed;
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
        characterState.IsInactive = true;
        rb.linearVelocity = Vector3.zero;

        if (duration > 0.01f)
        {
            StartCoroutine(IESetActive(duration));
        }
    }

    IEnumerator IESetActive(float duration)
    {
        yield return new WaitForSeconds(duration);

        characterState.IsInactive = false;

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

        if (rb.linearVelocity.y < -5f && !Physics.Raycast(tf.position, -tf.up, 10f, layerGround|layerStairGround))
        {
            characterState.IsOnGround = false;
            return true;
        }
        characterState.IsOnGround = true;
        return false;
    }

    public void SetCharacterOnStair( bool val)
    {
        characterState.IsOnStair = val;
        if(IsBot)collider.enabled = !val;
    }

    public virtual bool CheckCharacterOnStair()
    {   
        Debug.DrawLine(tf.position, tf.position - tf.up*10);
        bool result = Physics.Raycast(tf.position, -tf.up, 10f, layerStairGround);
        if(IsBot)collider.enabled = !result;
        
        
        return result;
    }
    public void CheckGate()
    {
        if (Physics.Raycast(tf.position, tf.forward, out RaycastHit hitGate, rangeDetect, layerGate))
        {
            Collider col = hitGate.collider;


            GateCtrl gate = ColliderCache<GateCtrl>.GetComponent(col);



            if (gate.NextStage == currentStage || gate.NextStage == null)
            {
                characterState.BlockDown = true;
            }
            else
            {
                characterState.BlockDown = false;
            }
        }
        else
        {
            characterState.BlockDown = false;
        }
    }

    public void CheckStair()
    {
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
                characterState.BlockForward = false;
            }
            else
            {
                characterState.BlockForward = true;
            }
        }
        else
        {
           characterState.BlockForward = false;
        }
    }

    public void CheckForward()
    {

        //Check Gate
        CheckGate();
        //Check stair
        CheckStair();

    }
    public virtual void Knockback(Vector3 knockbackDirection)
    {

        SetInActive();
        //gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
        rb.AddForce(knockbackDirection * knockForce, ForceMode.Impulse);
        tf.rotation = Quaternion.LookRotation(-knockbackDirection);
        if (brickCharacterManager.GetAmountRealBrick() > 0)
        {
            breakBrickEffect.transform.position = brickCharacterManager.GetBrickPosition(brickCharacterManager.VisualBrickId / 2);
            breakBrickEffect.Play();
        }
        brickCharacterManager.ClearBrick();
        ChangeAnim(GameData.Instance.ANIM_KNOCKBACK);

        EventBus<OnCharacterInActive>.Raise(new OnCharacterInActive
        {
            CharacterId = CharacterId
        });


        Invoke(nameof(StandUp), 2f);

    }

    public virtual void StandUp()
    {
        characterState.IsInactive = false;

        characterState.BlockForward = false;
        characterState.BlockForward = false;
        
    }


    public void OnCollisionEnter(Collision collision)
    {
        if(characterState.IsOnStair)return;

        if (characterState.IsInactive)
        {
            return;
        }
        
        Collider collider = collision.collider;
        if (collider.CompareTag(GameData.Instance.CHARACTER_TAG))
        {

            Character character = ColliderCache<Character>.GetComponent(collider);

            if (character.CharacterState.IsInactive) return;
            if(character.CharacterState.IsOnStair)return;

            Vector3 knockbackDirBA = character.tf.position - tf.position;
            Vector3 knockbackDirAB = tf.position - character.tf.position;
            knockbackDirAB.y = 0.8f;
            knockbackDirBA.y = 0.8f;
            knockbackDirAB.Normalize();
            knockbackDirBA.Normalize();
            if (character.BrickCharacterManager.GetAmountVisualBrick() < brickCharacterManager.GetAmountVisualBrick())
            {
                character.Knockback(knockbackDirBA);
                SetInActive(0.1f);


            }
            else if (character.brickCharacterManager.GetAmountVisualBrick() > brickCharacterManager.GetAmountVisualBrick())
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
        layerStairGround = LayerMask.GetMask("StairGround");

        tf = this.transform;
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }


        if (characterState.IsInactive)
        {
            return;
        }
        //characterState.IsOnStair = CheckCharacterOnStair();
       
    }
}
