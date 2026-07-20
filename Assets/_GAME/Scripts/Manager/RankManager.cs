using System.Collections.Generic;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    [SerializeField] private List<Character> rankedCharacters = new List<Character>();

    public void OnEnable()
    {
        EventBus<OnCharacterUpStage>.Subcribe(SortRankCharacter);
    }
    public void OnDisable()
    {
        EventBus<OnCharacterUpStage>.UnSubcribe(SortRankCharacter);
    }

    public List<Character> GetRankedList()
    {
        return rankedCharacters;
    }

    public void LoadRankedList(List<Character> characters)
    {
        rankedCharacters = new List<Character>(characters);
    }

    public void SortRankCharacter(OnCharacterUpStage onCharacterUpStage)
    {
        List<Character> newRank = new List<Character>();
        
        bool newRankAdded = false;
        for (int i = 0; i < rankedCharacters.Count; i++)
        {
            if(rankedCharacters[i].CurrentStage == null)
            {
                Debug.Log("BUG HERE");
            }
            if (onCharacterUpStage.Stage <= rankedCharacters[i].CurrentStage.StageNumber &&
            onCharacterUpStage.Character.CharacterId != rankedCharacters[i].CharacterId)
            {
                newRank.Add(rankedCharacters[i]);
            }
            else
            {
                if (!newRankAdded)
                {
                    newRank.Add(onCharacterUpStage.Character);
                    newRankAdded = true;
                }

                if (onCharacterUpStage.Character.CharacterId == rankedCharacters[i].CharacterId)
                {
                    continue;
                }
                else
                {
                    newRank.Add(rankedCharacters[i]);
                }
            }
        }

        //Xu li phat su kien rank change

        for (int indexNew = 0; indexNew < newRank.Count; indexNew++)
        {

            EventBus<OnRankChange>.Raise(new OnRankChange
            {
                CharacterId = newRank[indexNew].CharacterId,
                NewRank = indexNew + 1

            });

        }

        rankedCharacters = newRank;
    }

    public Character GetCharacterRank(int rank)
    {
        return rankedCharacters[rank - 1];
    }

}
