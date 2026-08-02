using System;
using System.Collections.Generic;
using UnityEngine;

public class BrickCharacterManager : MonoBehaviour
{
    [SerializeField] private Character character;

    [SerializeField] private Transform brickRoot;
    [SerializeField] protected List<Brick> characterBricks = new List<Brick>();

    [SerializeField] private Vector3 startCharacterBrickPos;

    [SerializeField] private int visualBrickId = 0;

    public void SetBrickRoot(Transform tf)
    {
        brickRoot = tf;
    }
     public int GetNextBrickIndex()
    {
        int assignedIndex = visualBrickId;
        visualBrickId += 1;
        return assignedIndex;
    }

    public int GetVisualBrickId()
    {
        return visualBrickId;
    }

    public void RemoveBrickIndex()
    {

        visualBrickId = Math.Max(0, visualBrickId - 1);
    }
    public Vector3 GetBrickPosition(int index)
    {

        return startCharacterBrickPos + new Vector3(0f, index * (GameData.Instance.BRICK_SIZE.y / 2 + 0.05f), 0f) + brickRoot.position;
    }
    public int GetAmountVisualBrick()
    {
        return visualBrickId;
    }

    public int GetAmountRealBrick()
    {
        return characterBricks.Count;
    }

    public void AddBrick()
    {
        SoundManager.Instance.PlaySFXCollectBrick();
        Vector3 localPos = startCharacterBrickPos + new Vector3(0f, characterBricks.Count * (GameData.Instance.BRICK_SIZE.y / 2 + 0.05f), 0f);
        Brick brick = SimplePool.Spawn<Brick>(PoolType.BrickPool, Vector3.zero, Quaternion.identity);

        brick.OnInit();
        brick.SetLocal(localPos, Quaternion.identity, brickRoot);
        brick.SetColor(character.ColorType);
        brick.SetActiveTrail(false);
        characterBricks.Add(brick);
        BrickEffect brickEffect = SimplePool.Spawn<BrickEffect>(PoolType.BrickEffectPool, Vector3.zero, Quaternion.identity);
        brickEffect.SetColor(character.ColorType);
        brickEffect.SetLocal(localPos, Quaternion.identity, brickRoot);
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
        character.CurrentStage.ReSpawnBrick(brick.ColorType);
        visualBrickId -= 1;
        brick.OnDespawn();
    }

    public void ClearBrick()
    {
        while (characterBricks.Count > 0)
        {
            RemoveBrick();
        }
    }
}
