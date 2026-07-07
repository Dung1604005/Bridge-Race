

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class Stage : GameUnit
{

    [SerializeField] private StageDataSO stageDataSO;
    public int StageNumber;

    public bool IsLastStage;
    [SerializeField] private float scaleX;

    [SerializeField] private float scaleY;

    [SerializeField] private float scaleZ;

    [SerializeField] private Vector3 sizeStage;

    [SerializeField] private Vector3 distanceBrick;

    [SerializeField] private List<Transform> spawnPos = new List<Transform>();

    [SerializeField] private List<Character> characters = new List<Character>();

    [SerializeField] private List<Bridge> bridges = new List<Bridge>();

    private Dictionary<ColorType, List<Brick>> bricks = new Dictionary<ColorType, List<Brick>>();

    private Dictionary<Brick, int> flyingBricks = new Dictionary<Brick, int>();

    [ContextMenu("Create Data")]

    public void CreateData()
    {
        StageData stageData = new StageData();

        stageData.TFData = Helper.CreateDataFromTransform(tf);

        Debug.Log(stageData.TFData.EulerAngles);

        stageData.StageNumber = StageNumber;

        stageData.IsLastStage = IsLastStage;
        stageData.ScaleX=scaleX;
        stageData.ScaleY = scaleY;
        stageData.ScaleZ = scaleZ;


        TransformData[] SpawnPos = new TransformData[spawnPos.Count];
        for(int i = 0; i < SpawnPos.Length; i++)
        {
            SpawnPos[i] = Helper.CreateDataFromTransform(spawnPos[i]);
        }

        stageData.SpawnPos = SpawnPos;

        stageDataSO.StageData = stageData;

         EditorUtility.SetDirty(stageDataSO);
        AssetDatabase.SaveAssets();

        
    }

    public void LoadData(StageDataSO stageDataSO)
    {
        Helper.LoadTransformData(tf, stageDataSO.StageData.TFData);

        IsLastStage = stageDataSO.StageData.IsLastStage;

        StageNumber = stageDataSO.StageData.StageNumber;

        scaleX = stageDataSO.StageData.ScaleX;
        scaleY = stageDataSO.StageData.ScaleY;
        scaleZ = stageDataSO.StageData.ScaleZ;

        //Vi size goc la 2 don vi nen *2
        sizeStage = (new Vector3(scaleX, scaleY, scaleZ)) * 2;  

        Debug.Log(spawnPos.Count + " " + stageDataSO.StageData.SpawnPos.Length);
        for(int i = 0; i < stageDataSO.StageData.SpawnPos.Length; i++)
        {
            Helper.LoadTransformData(spawnPos[i], stageDataSO.StageData.SpawnPos[i]);
        }
        SpawnBrick(LevelManager.Instance.ListColors);
    }

    public void LoadDataBridge(StageDataSO stageDataSO)
    {
        for(int i = 0; i < stageDataSO.BridgeDatas.Count; i++)
        {
            TransformData tfData = stageDataSO.BridgeDatas[i].bridgeTFData;

            Bridge newBridge= SimplePool.Spawn<Bridge>(PoolType.BridgePool, tfData.Position, Quaternion.identity);
            newBridge.TF.SetParent(LevelManager.Instance.LevelRoot, true);
            newBridge.OnInit();
            newBridge.LoadData(stageDataSO.BridgeDatas[i]);
            bridges.Add(newBridge);
            
        }
    }
    public void OnInit()
    {      
        characters.Clear();
        bridges.Clear();
        ClearAllBrick();
    }

    public void OnDespawn()
    {
        spawnPos.Clear();
        characters.Clear();
        bridges.Clear();
        ClearAllBrick();
    }

    public void OnWin()
    {
        foreach(List<Brick> listBrick in bricks.Values)
        {
            foreach(Brick brick in listBrick)
            {
                brick.OnWin();
            }
        }
    }

    public Vector3 GetSpawnPosCharacter(Character character)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if(character == characters[i])
            {
                return spawnPos[i].position;

            }
        }
        
        return spawnPos[0].position;
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
            brick.SetCollected(false);
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
                // Khong despawn do co the dan den brick cua nhan vat dung brick cua stage => bug
                //brick.SetActive(false);
                brick.OnDespawn();
            }
            
            bricks[colorType].Clear();
            
        }
        else
        {
            Debug.LogError("Color brick want to be deactive dont have in stage!!!");
        }

    }

    public void ClearAllBrick()
    {
        foreach(ColorType colorType in bricks.Keys)
        {
            for(int i = 0; i < bricks[colorType].Count; i++)
            {
                bricks[colorType][i].OnDespawn();
            }
        }

        foreach(Brick brick in flyingBricks.Keys)
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
                amount+= 1;
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
                    int colorRand =  UnityEngine.Random.Range(0, 4);
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
            int rad = UnityEngine.Random.Range(0, possibleAns.Count);
            return possibleAns[rad];
        }

    }

    

    void Awake()
    {
        tf = this.transform;

    }

    void Update()
    {
        
        for (int i = 0; i < characters.Count; i++)
        {
            Character character = characters[i];
            if(character.IsInActive)continue;
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
                        
                        brick.Move(character, character.GetBrickPosition(flyingBricks[brick]));

                    }


                }
            }
        }


    }
}


[Serializable]
public struct StageData
{
    public int StageNumber;

    public bool IsLastStage;

    public float ScaleX;

    public float ScaleY;

    public float ScaleZ;

    public TransformData[] SpawnPos;
    public TransformData TFData;
}
