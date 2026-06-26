
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] private ColorType colorType;

    [SerializeField] private Renderer renderer;

    [SerializeField] private TrailRenderer[] trailRenderers;

    [SerializeField] private float radCollect;

    [SerializeField] private float distanceBehind;

    [SerializeField] private float moveSpeed;

    [SerializeField] private float accelerate;

    [SerializeField] private float rotationSpeed;

    private Vector3 spawnPos;
    private Stage stage;

    private bool isCollected = false;

    private bool reachBehind = false;

    private float flyTimer = 0f;

    public float RadCollect => radCollect;

    public bool IsCollected => isCollected;
    public ColorType ColorType => colorType;


    public void OnInit(Stage _stage, Vector3 _spawnPos)
    {
        stage = _stage;
        spawnPos = _spawnPos;
        isCollected = false;
        reachBehind = false;
        flyTimer = 0f;
    }

    public void OnDespawn()
    {
        stage = null;
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
         foreach(TrailRenderer trail in trailRenderers)
        {
            trail.material = GameData.Instance.ColorDataSO.GetColorParticalMaterial(colorType);
        }
        
    }

    public void SetActiveTrail(bool active)
    {
        foreach(TrailRenderer trail in trailRenderers)
        {
            trail.enabled = active;
        }
        
    }

    public void SetActive(bool active)
    {

        tf.gameObject.SetActive(active);
    }


    public void SetCollected(bool _isCollected)
    {
        isCollected = _isCollected;
        
    }

    public void Move(Character character, Vector3 targetPosition)
    {
        flyTimer += Time.deltaTime;
        tf.rotation = Quaternion.Slerp(tf.rotation, character.TF.rotation, rotationSpeed * Time.deltaTime);
        
        if (!reachBehind)
        {
            //Bay ve 1 diem phia sau lung cua player truoc
            Vector3 characterBehind = -character.TF.forward * distanceBehind + character.TF.position;
            targetPosition.x = characterBehind.x;
            targetPosition.z = characterBehind.z;
        }
        
        tf.position = Vector3.MoveTowards(tf.position, targetPosition, (moveSpeed + accelerate*flyTimer) * Time.deltaTime);
        
        if ((tf.position - targetPosition).sqrMagnitude <= 0.01f)
        {
            if (!reachBehind)
            {
                reachBehind = true;
            }
            else
            {
                stage.RemoveFlyingBrick(this);
                character.AddBrick();
                flyTimer = 0f;
                SetActive(false);
            }


        }
    }

    void Awake()
    {
        tf = this.transform;
    }
}
