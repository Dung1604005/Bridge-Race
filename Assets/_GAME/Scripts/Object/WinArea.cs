using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
   [SerializeField] private List<Transform> rankPositions = new List<Transform>();


   public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(GameData.Instance.CHARACTER_TAG))
        {
            Character character = ColliderCache<Character>.GetComponent(collider);
            Debug.Log("WINNING");
            EventBus<OnWin>.Raise(new OnWin{});
            character.SetSpawn(rankPositions[0].position);

            LevelManager.Instance.GetCharacterRank(2).SetSpawn(rankPositions[1].position);
            LevelManager.Instance.GetCharacterRank(3).SetSpawn(rankPositions[2].position);
            
        }
    }

    
}
