
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public int StageNumber;
    [SerializeField] private float scaleX;

    [SerializeField] private float scaleY;

    [SerializeField] private float scaleZ;

    [SerializeField] private Vector3 sizeStage;

    [SerializeField] private Vector3 distanceBrick;

    [SerializeField] private List<Vector3> spawnPos = new List<Vector3>();

    [SerializeField] private List<Character> characters;

    [SerializeField] private List<Bridge> bridges;


    private Dictionary<ColorType, List<Brick>> bricks = new Dictionary<ColorType, List<Brick>>();

    private Dictionary<Brick, int> flyingBricks = new Dictionary<Brick, int>();

    private Transform tf;

    public void OnInit()
    {
        tf.localScale = new Vector3(scaleX, scaleY, scaleZ);

        //Vi size goc la 2 don vi nen *2
        sizeStage = (new Vector3(scaleX, scaleY, scaleZ)) * 2;

    }

    public Vector3 GetSpawnPosCharacter(Character character)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if(character == characters[i])
            {
                return spawnPos[i];

            }
        }
        
        return spawnPos[0];
    }

    public void AddCharacter(Character character)
    {
        characters.Add(character);
        ActiveBrickByColor(character.ColorType);

    }

    public void RemoveCharacter(Character character)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] == character)
            {
                DeActiveBrickByColor(character.ColorType);
                characters.RemoveAt(i);
               

                return;
            }
        }
    }

    public void RemoveFlyingBrick(Brick brick)
    {
        if (flyingBricks.ContainsKey(brick))
        {
            flyingBricks.Remove(brick);
        }
    }

    public void ActiveBrickByColor(ColorType colorType)
    {
        if (bricks.ContainsKey(colorType))
        {
            foreach (Brick brick in bricks[colorType])
            {
                brick.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Color brick want to be active dont have in stage!!!");
        }

    }

    public void DeActiveBrickByColor(ColorType colorType)
    {
        if (bricks.ContainsKey(colorType))
        {
            foreach (Brick brick in bricks[colorType])
            {
                SimplePool.Despawn(brick);
            }
            
        }
        else
        {
            Debug.LogError("Color brick want to be deactive dont have in stage!!!");
        }

    }

    public Vector3 GetNearestBrick(ColorType colorType, Vector3 pos)
    {
        float minDis = 1000000000f;
        Vector3 ans = pos;

        foreach (Brick brick in bricks[colorType])
        {
            if (brick.gameObject.activeSelf && !flyingBricks.ContainsKey(brick) && (brick.TF.position - pos).sqrMagnitude < minDis)
            {
                minDis = (brick.TF.position - pos).sqrMagnitude;
                ans = brick.TF.position;
            }
        }
        return ans;
    }


    public int GetAmountActiveBrick(ColorType colorType)
    {
        int amount = 0;
        if (!bricks.ContainsKey(colorType))
        {
            return 0;
        }
        foreach (Brick brick in bricks[colorType])
        {
            amount += 1;
        }

        return amount;
    }

    public void ReSpawnBrick(ColorType colorType)
    {
        if (!bricks.ContainsKey(colorType))
        {
            return;
        }
        foreach (Brick brick in bricks[colorType])
        {
            if (!brick.gameObject.activeSelf)
            {
                brick.OnInit();
                return;
            }
        }
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
                brick.SetInfor(this, pos);
                brick.OnInit();
                brick.SetActive(false);
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


    public Bridge GetBestBridge(ColorType colorType)
    {
        int maxColorStair = 0;
        List<Bridge> possibleAns = new List<Bridge>();
        for (int i = 0; i < bridges.Count; i++)
        {
            int amountColorStair = bridges[i].GetAmountColorStair(colorType);
            if (amountColorStair > maxColorStair)
            {
                maxColorStair = amountColorStair;
                possibleAns.Clear();
                possibleAns.Add(bridges[i]);
            }
            else if (amountColorStair == maxColorStair)
            {
                possibleAns.Add(bridges[i]);
            }
        }
        if (possibleAns.Count == 1)
        {
            return possibleAns[0];
        }
        else
        {
            int rad = Random.Range(0, possibleAns.Count);
            return possibleAns[rad];
        }

    }

    void Awake()
    {
        tf = this.transform;
        OnInit();

    }
    void Start()
    {
        SpawnBrick(new List<ColorType>() { ColorType.RED, ColorType.BLUE, ColorType.VIOLET, ColorType.GREEN });
        foreach(Character character in characters)
        {
            ActiveBrickByColor(character.ColorType);
        }
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
                                flyingBricks.Add(brick, character.GetNextBrickIndex());
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
