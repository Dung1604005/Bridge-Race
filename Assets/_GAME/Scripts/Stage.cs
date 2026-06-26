using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private float scaleX;

    [SerializeField] private float scaleY;

    [SerializeField] private float scaleZ;

    [SerializeField] private Vector3 sizeStage;

    private Dictionary<ColorType, List<Brick>> bricks = new Dictionary<ColorType, List<Brick>>();
    private Dictionary<Brick, int> flyingBricks = new Dictionary<Brick, int>();

    [SerializeField] private Vector3 distanceBrick;

    [SerializeField] private List<Character> characters;

    private Transform tf;

    public void OnInit()
    {
        tf.localScale = new Vector3(scaleX, scaleY, scaleZ);

        //Vi size goc la 2 don vi nen *2
        sizeStage = (new Vector3(scaleX, scaleY, scaleZ)) * 2;

    }

    public void RemoveFlyingBrick(Brick brick)
    {
        if (flyingBricks.ContainsKey(brick))
        {
            flyingBricks.Remove(brick);
        }
    }

    public Vector3 GetNearestBrick(ColorType colorType, Vector3 pos)
    {
        float minDis = 1000000000f;
        Vector3 ans = pos;

         foreach (Brick brick in bricks[colorType])
        {
            if(brick.gameObject.activeSelf && !flyingBricks.ContainsKey(brick) && (brick.TF.position - pos).sqrMagnitude < minDis)
            {
                minDis = (brick.TF.position - pos).sqrMagnitude ;
                ans = brick.TF.position;
            }
        }
        return ans;
    }

    public int GetAmountActiveBrick(ColorType colorType)
    {
        int amount = 0;
         foreach (Brick brick in bricks[colorType])
        {
            if (brick.gameObject.activeSelf)
            {
                amount+= 1;
            }
        }
        return amount;
    }

    public void SpawnBrick(List<ColorType> colorTypes)
    {
        if (colorTypes.Count != 4)
        {
            Debug.LogError("STAGE DONT HAVE ENOUGH COLOR");
            return;
        }
        int numbCollumn = (int)((sizeStage.x - distanceBrick.x) / (GameData.Instance.BRICK_SIZE.x + distanceBrick.x));
        int numbRow = (int)((sizeStage.z - distanceBrick.z) / (GameData.Instance.BRICK_SIZE.z + distanceBrick.z));
        int numbBrick = numbCollumn * numbRow;

        //Moi stage se co 4 mau va spawn so luong brick cua tung loai mau giong nhau
        List<int> numbColorBrick = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            numbColorBrick.Add(numbBrick / 4);
        }


        Vector3 leftBottomPos = tf.position - new Vector3(sizeStage.x / 2, 0f, sizeStage.z / 2);

        for (int x = 0; x < numbCollumn; x += 1)
        {
            for (int z = 0; z < numbRow; z += 1)
            {
                Vector3 pos = new Vector3((x + 1) * distanceBrick.x + x * GameData.Instance.BRICK_SIZE.x + leftBottomPos.x + GameData.Instance.BRICK_SIZE.x / 2,
                tf.position.y + sizeStage.y + GameData.Instance.BRICK_SIZE.y / 2,
                (z + 1) * distanceBrick.z + z * GameData.Instance.BRICK_SIZE.z + leftBottomPos.z + GameData.Instance.BRICK_SIZE.z / 2);
                Brick brick = SimplePool.Spawn<Brick>(PoolType.BrickPool, pos, Quaternion.identity);
                brick.OnInit(this, pos);
                for (int timer = 0; timer <= 100; timer++)
                {
                    int colorRand = Random.Range(0, 4);
                    if (numbColorBrick[colorRand] > 0)
                    {
                        numbColorBrick[colorRand] -= 1;

                        brick.SetColor(colorTypes[colorRand]);
                        if (bricks.ContainsKey(colorTypes[colorRand]))
                        {
                            bricks[colorTypes[colorRand]].Add(brick);
                        }
                        else
                        {
                            bricks.Add(colorTypes[colorRand], new List<Brick>() { brick });
                        }
                        break;
                    }
                }
            }
        }


    }

    void Awake()
    {
        tf = this.transform;
        OnInit();
        SpawnBrick(new List<ColorType>() { ColorType.RED, ColorType.BLUE, ColorType.VIOLET, ColorType.GREEN });
    }
    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            Character character = characters[i];
            foreach (Brick brick in bricks[character.ColorType])
            {
                if (brick.gameObject.activeSelf)
                {

                    if (!brick.IsCollected)
                    {
                        float distance = (brick.TF.position - character.TF.position).sqrMagnitude;
                        if (distance * distance + 0.001f < brick.RadCollect * brick.RadCollect)
                        {
                            brick.SetCollected(true);
                            if (!flyingBricks.ContainsKey(brick))
                            {
                                flyingBricks.Add(brick, character.GetBrickIndex());
                            }
                        }
                    }
                    if (brick.IsCollected)
                    {

                        brick.Move(character, character.GetNextBrickPosition(flyingBricks[brick]));

                    }


                }
            }
        }
        
        
    }
}
