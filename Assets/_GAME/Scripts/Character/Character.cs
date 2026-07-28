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
    [SerializeField] private ColorType colorType;


    public bool IsBot;


    [Header("REFERENCE")]

    [SerializeField] protected TextMeshProUGUI textName;

    [SerializeField] protected Stage currentStage;

    [SerializeField] protected Animator anim;

    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected BrickCharacterManager brickCharacterManager;

    [SerializeField] protected CharacterKnockBack characterKnockBack;

    [SerializeField] protected CharacterChecker characterChecker;

    [SerializeField] private Renderer renderer;

    [SerializeField] protected Transform tf;

    [SerializeField] protected Transform tfVisual;

    [SerializeField] protected CharacterState characterState;

    [SerializeField] private SkinController skinPrefab;


    private String currentAnim = "";

    public BrickCharacterManager BrickCharacterManager => brickCharacterManager;

    public CharacterChecker CharacterChecker => characterChecker;


    public Transform TF => tf;

    public Rigidbody Rb => rb;
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
        return characterChecker.CharacterIsGoingDown();
    }

    public virtual bool CharacterIsFalling()
    {
        return characterChecker.CharacterIsFalling();
    }
    public virtual void Knockback(Vector3 knockbackDirection)
    {
        characterKnockBack.Knockback(knockbackDirection);
        Invoke(nameof(StandUp), 2f);
    }
    public virtual void StandUp()
    {
        characterKnockBack.StandUp();
        
    }
    protected virtual void Awake()
    {
        characterChecker.OnInit();
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
        characterChecker.CharacterIsFalling();
       
    }
}
