
using UnityEngine;

public interface IEvent
{
    
}

public struct OnCharacterUpStage: IEvent
{
    public Character Character;

    public int Stage;
}

public struct OnStairChange: IEvent
{
    public int CharacterId;

    public int StairId;
}

public struct OnCharacterInActive: IEvent
{
    public int CharacterId;
}

public struct OnRankChange: IEvent
{
    public int CharacterId;
    public int NewRank;
}