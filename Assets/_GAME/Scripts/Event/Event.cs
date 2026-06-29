
using UnityEngine;

public interface IEvent
{
    
}

public struct OnCharacterUpStage: IEvent
{
    public Character Character;

    public int Stage;
}

public struct OnWin: IEvent
{
    
}

public struct OnMapLoadComplete: IEvent
{
    
}