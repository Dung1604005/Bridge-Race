using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
   [SerializeField] private List<Transform> rankPositions = new List<Transform>();

   public Transform TF {get; private set;}


   public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(GameData.Instance.CHARACTER_TAG))
        {
            Character character = ColliderCache<Character>.GetComponent(collider);
            EventBus<OnCharacterUpStage>.Raise(new OnCharacterUpStage
            {
                Character = character,
                Stage = 10
            });
            EventBus<OnWin>.Raise(new OnWin{});
            
            character.SetSpawn(rankPositions[0].position);

            LevelManager.Instance.GetCharacterRank(2).SetSpawn(rankPositions[1].position);
            LevelManager.Instance.GetCharacterRank(3).SetSpawn(rankPositions[2].position);
            LevelManager.Instance.GetCharacterRank(4).SetSpawn(rankPositions[3].position);
            
        }
    }

    void Awake()
    {
        TF= transform;
    }


}
