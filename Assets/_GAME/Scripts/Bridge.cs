using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private List<Stair> stairs = new List<Stair>();

    [SerializeField] private Stage ownerStage;

    [SerializeField] private Stage nextStage;

    public Stage NextStage => nextStage;

    public Stage OwnerStage => ownerStage;

    public List<Stair> Stairs => stairs;

    public void SetOwnerStage(Stage _ownerStage)
    {
        ownerStage = _ownerStage;
    }

    public void SetNextStage(Stage _nextStage)
    {
        nextStage = _nextStage;
    }

    public int GetAmountColorStair(ColorType colorType)
    {
        int ans = 0;
        for(int i = 0; i < stairs.Count; i++)
        {
            if(stairs[i].ColorType == colorType)
            {
                ans += 1;
            }
        }
        return ans;
    }

    public StairInfo GetFarthestStairPossible(int currentStair, ColorType colorType, int number)
    {
        
        int farthestStair = currentStair;
        for(int i = currentStair + 1; i < stairs.Count; i++)
        {
            if(stairs[i].ColorType != colorType)
            {
                if(number == 0)
                {
                    return new StairInfo
                    {
                        stairId = farthestStair,
                        isLastStair = (farthestStair == stairs.Count - 1) ? true:false,
                        position = stairs[farthestStair].transform.position
                    };
                }
                number -= 1;
            }
            farthestStair = i;

        }
        return new StairInfo
        {
           stairId = farthestStair,
           isLastStair = (farthestStair == stairs.Count - 1) ? true:false,
           position = stairs[farthestStair].transform.position  
        };
    }

    void Start()
    {
        foreach(Stair stair in stairs)
        {
            stair.SetBridge(this);
        }
    }

}


public struct StairInfo
{
    public int stairId;

    public bool isLastStair;
    public Vector3 position;
}