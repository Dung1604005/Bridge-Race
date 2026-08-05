using System.Collections.Generic;
using UnityEngine;

public class StageBrickManager : MonoBehaviour
{
    [SerializeField] private Stage stage;
    [SerializeField] private float scaleX;

    [SerializeField] private float scaleY;

    [SerializeField] private float scaleZ;

    private Dictionary<ColorType, List<Brick>> bricks = new Dictionary<ColorType, List<Brick>>();

    private Dictionary<Brick, int> flyingBricks = new Dictionary<Brick, int>();

    public float ScaleX => scaleX;

    public float ScaleY => scaleY;

    public float ScaleZ => scaleZ;

    public Dictionary<ColorType, List<Brick>> Bricks => bricks;

    public void SetScale(float scaleX, float scaleY, float scaleZ)
    {
        this.scaleX = scaleX;
        this.scaleY = scaleY;
        this.scaleZ = scaleZ;
    }

    public void RemoveFlyingBrick(Brick brick)
    {
        if (flyingBricks.ContainsKey(brick))
        {
            flyingBricks.Remove(brick);
            brick.SetCollected(false);
        }
    }

    public void SetActiveBrickByColor(ColorType colorType, bool active)
    {
        if (bricks.ContainsKey(colorType))
        {
            foreach (Brick brick in bricks[colorType])
            {
                if (active)
                {
                    brick.SetActive(true);
                }
                else
                {
                    brick.OnDespawn();
                }
            }
            if (!active)
            {
                bricks[colorType].Clear();
            }
        }
        else
        {
            Debug.LogError("Color brick want to be active dont have in stage!!! " + colorType);
        }

    }

    public void ClearAllBrick()
    {
        foreach (ColorType colorType in bricks.Keys)
        {
            for (int i = 0; i < bricks[colorType].Count; i++)
            {
                bricks[colorType][i].OnDespawn();
            }
        }

        foreach (Brick brick in flyingBricks.Keys)
        {
            brick.OnDespawn();
        }

        flyingBricks.Clear();
        bricks.Clear();
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
            if (brick.gameObject.activeSelf)
            {
                amount += 1;
            }

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


    [ContextMenu("CHECK VALID STAGE")]
    public void CheckValidSize()
    {
        Vector3 distanceBrick = GameConfig.DISTANCE_BRICK;
        int numbCollumn = (int)((scaleX * 2 - distanceBrick.x) / (GameConfig.BRICK_SIZE.x + distanceBrick.x));
        int numbRow = (int)((scaleZ * 2 - distanceBrick.z) / (GameConfig.BRICK_SIZE.z + distanceBrick.z));
        int numbBrick = numbCollumn * numbRow;

        if (numbBrick % 4 == 0)
        {
            Debug.Log("VALID STAGE");
        }
        else
        {
            Debug.Log("UNVALID STAGE");
        }
    }

    public void UpdateBrickCollection()
    {
        for (int i = 0; i < stage.Characters.Count; i++)
        {
            Character character = stage.Characters[i];
            if(character.CharacterState.GetIsInActive())continue;
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
                                flyingBricks.Add(brick, character.BrickCharacterManager.GetNextBrickIndex());
                            }
                        }
                    }
                    if (brick.IsCollected)
                    {
                        brick.Move(character, character.BrickCharacterManager.GetBrickPosition(flyingBricks[brick]));
                    }
                }
            }
        }
    }

    public Vector3 CaculateBrickPos(Vector3 leftBottomPos, int x, int z)
    {
        Vector3 distanceBrick = GameConfig.DISTANCE_BRICK;
        Vector3 pos = new Vector3((x + 1) * distanceBrick.x + x * GameConfig.BRICK_SIZE.x + leftBottomPos.x + GameConfig.BRICK_SIZE.x / 2,
               stage.TF.position.y + scaleY*2 + GameConfig.BRICK_SIZE.y / 2,
               (z + 1) * distanceBrick.z + z * GameConfig.BRICK_SIZE.z + leftBottomPos.z + GameConfig.BRICK_SIZE.z / 2);
        return pos;
    }

    public Brick SpawnBrickObject(List<int> numbColorBrick, List<ColorType> colorTypes, Vector3 leftBottomPos, int x, int z)
    {
        
        Vector3 pos = CaculateBrickPos(leftBottomPos, x, z);
        Brick brick = SimplePool.Spawn<Brick>(PoolType.BrickPool, pos, Quaternion.identity);
        brick.SetInfor(stage, pos);
        brick.OnInit();
        brick.SetActive(false);
        for (int timer = 0; timer <= 100; timer++)
        {
            int colorRand = UnityEngine.Random.Range(0, 4);
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
        return brick;
    }
    public void SpawnBrickStage(List<ColorType> colorTypes)
    {
        Vector3 distanceBrick = GameConfig.DISTANCE_BRICK;
        int numbCollumn = (int)((scaleX * 2 - distanceBrick.x) / (GameConfig.BRICK_SIZE.x + distanceBrick.x));
        int numbRow = (int)((scaleZ * 2 - distanceBrick.z) / (GameConfig.BRICK_SIZE.z + distanceBrick.z));
        int numbBrick = numbCollumn * numbRow;

        //Moi stage se co 4 mau va spawn so luong brick cua tung loai mau giong nhau
        List<int> numbColorBrick = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            numbColorBrick.Add(numbBrick / 4);
        }

        Vector3 leftBottomPos = stage.TF.position - new Vector3(scaleX, 0f, scaleZ);

        for (int x = 0; x < numbCollumn; x += 1)
        {
            for (int z = 0; z < numbRow; z += 1)
            {
                Brick brick = SpawnBrickObject(numbColorBrick, colorTypes, leftBottomPos, x, z);
                if (bricks.ContainsKey(brick.ColorType))
                {
                    bricks[brick.ColorType].Add(brick);
                }
                else
                {
                    bricks.Add(brick.ColorType, new List<Brick>() { brick });
                }
            }
        }


    }
}
