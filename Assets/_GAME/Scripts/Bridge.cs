using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private List<Stair> stairs = new List<Stair>();

    public void OnInit()
    {
        
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
           position = stairs[farthestStair].transform.position  
        };
    }

}


public struct StairInfo
{
    public int stairId;
    public Vector3 position;
}