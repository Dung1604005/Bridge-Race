using System;
using UnityEngine;

[Serializable]
public class CharacterState
{
    public bool IsInactive;
    public bool IsOnGround;
    public bool IsOnStair;
    public bool BlockForward;
    public bool BlockDown;

    public CharacterState()
    {
        IsInactive = false;
        IsOnGround = true;
        IsOnStair = false;
        BlockForward = false;
        BlockDown = false;
    }

    public void SetIsInActive(bool isInActive)
    {
        IsInactive = isInActive;
    }

    public void SetIsOnGround(bool isOnGround)
    {
        IsOnGround = isOnGround;
    }

    public void SetIsOnStair(bool isOnStair)
    {
        IsOnStair = isOnStair;
    }

    public void SetBlockForward(bool blockForward)
    {
        BlockForward = blockForward;
    }

    public void SetBlockDown(bool blockDown)
    {
        BlockDown = blockDown;
    }
}