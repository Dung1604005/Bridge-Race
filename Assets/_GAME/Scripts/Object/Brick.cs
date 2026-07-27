
using System.Collections;
using UnityEngine;

public class Brick : GameUnit
{
    [Header("STAT")]
    [SerializeField] private ColorType colorType;

    [SerializeField] private float radCollect;

    [SerializeField] private float distanceBehind;

    [SerializeField] private float moveSpeed;

    [SerializeField] private float accelerate;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private float maxAngleShake;

    [SerializeField] private float speedShake;

    [SerializeField] private Vector3 spawnPos;

    [Header("REFERENCE")]

    [SerializeField] private Renderer renderer;

    [SerializeField] private TrailRenderer[] trailRenderers;

    [SerializeField] private Stage stage;

    [SerializeField] private Character targetCharacter;

    private bool isCollected = false;

    private bool reachBehind = false;

    private float flyTimer = 0f;

    public float RadCollect => radCollect;

    public bool IsCollected => isCollected;
    public ColorType ColorType => colorType;

    public void OnEnable()
    {
        EventBus<OnCharacterInActive>.Subcribe(ReSpawn);
    }

    public void OnDisable()
    {
        EventBus<OnCharacterInActive>.UnSubcribe(ReSpawn);
    }
    public void OnWin()
    {
        EndFlying();
        SetActive(false);
    }

    public void EndFlying()
    {
        if (stage != null)
        {
            stage.RemoveFlyingBrick(this);
        }
        flyTimer = 0f;
    }

    public void SetLocal(Vector3 position, Quaternion quaternion, Transform parent = null)
    {
        if (parent != null)
        {
            tf.SetParent(parent, true);
        }

        tf.localPosition = position;
        tf.localRotation = quaternion;
    }

    public void SetInfor(Stage _stage, Vector3 _spawnPos)
    {
        stage = _stage;
        spawnPos = _spawnPos;
    }

    public void OnInit()
    {
        
        targetCharacter = null;
        SetActive(true);
        tf.parent= null;
        tf.position = spawnPos;
        tf.rotation = Quaternion.identity;
        isCollected = false;
        reachBehind = false;
        flyTimer = 0f;
        foreach (TrailRenderer trail in trailRenderers)
        {
            trail.Clear();
        }
    }

    public void OnDespawn()
    {
        stage = null;
        tf.parent= null;
        
        targetCharacter = null;
        flyTimer = 0f;
        isCollected = false;
        reachBehind = false;
        SimplePool.Despawn(this);
    }

    public void SetColor(ColorType _colorType)
    {
        colorType = _colorType;
        if (renderer == null)
        {
            Debug.LogError("BRICK HAVENT SET RENDERER");
        }
        renderer.material = GameData.Instance.ColorDataSO.GetColorMaterial(colorType);
        foreach (TrailRenderer trail in trailRenderers)
        {
            trail.material = GameData.Instance.ColorDataSO.GetColorParticalMaterial(colorType);
        }

    }

    public void SetActiveTrail(bool active)
    {
        foreach (TrailRenderer trail in trailRenderers)
        {
            trail.enabled = active;
        }

    }

    public void ReSpawn(OnCharacterInActive onCharacterInActive)
    {
        
        if (targetCharacter != null && targetCharacter.CharacterId == onCharacterInActive.CharacterId && flyTimer > 0.01f)
        {
            EndFlying();
            if (targetCharacter != null)
            {
                targetCharacter.BrickCharacterManager.RemoveBrickIndex();
            }
            OnInit();
        }
    }

    IEnumerator IEFadeOut(float duration)
    {
        float timer = 0f;
        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.b, renderer.material.color.g, 0f);
        Color target = new Color(renderer.material.color.r, renderer.material.color.b, renderer.material.color.g, 255f);
        while (timer + 0.01f < duration)
        {
            timer += Time.deltaTime;

            renderer.material.color = Color.Lerp(renderer.material.color, target, timer / duration);
            yield return null;
        }

    }

    public void SetActive(bool active, bool haveTransition = false)
    {
        
        tf.gameObject.SetActive(active);
        if (haveTransition)
        {
            StartCoroutine(IEFadeOut(1f));
        }
    }


    public void SetCollected(bool _isCollected)
    {
        isCollected = _isCollected;

    }

    public void Move(Character character, Vector3 targetPosition)
    {
        if(GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }
        if (character.CharacterState.IsInactive)
        {
            EndFlying();
            OnInit();
            return;
        }
        flyTimer += Time.deltaTime;
        tf.rotation = Quaternion.Slerp(tf.rotation, character.TF.rotation, rotationSpeed * Time.deltaTime);
        targetCharacter = character;
        if (!reachBehind)
        {
            //Bay ve 1 diem phia sau lung cua player truoc
            Vector3 characterBehind = -character.TF.forward * distanceBehind + character.TF.position + Vector3.up*5f;
            targetPosition.x = characterBehind.x;
            targetPosition.z = characterBehind.z;
        }

        tf.position = Vector3.MoveTowards(tf.position, targetPosition, (moveSpeed + accelerate * flyTimer) * Time.deltaTime);

        if ((tf.position - targetPosition).sqrMagnitude <= 0.01f)
        {
            if (!reachBehind)
            {
                reachBehind = true;
            }
            else
            {
                SetActive(false);
                EndFlying();
                character.BrickCharacterManager.AddBrick();
                

            }


        }
    }
    void Awake()
    {
        tf = this.transform;
    }
}
