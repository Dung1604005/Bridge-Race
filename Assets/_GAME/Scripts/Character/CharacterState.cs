using System;
using UnityEngine;

[Serializable]
public class CharacterState
{
    [SerializeField]private bool isInactive;
    [SerializeField]private bool isOnGround;
    [SerializeField]private bool isOnStair;
    [SerializeField]private bool blockForward;
    [SerializeField]private bool blockDown;

    public CharacterState()
    {
        isInactive = false;
        isOnGround = true;
        isOnStair = false;
        blockForward = false;
        blockDown = false;
    }

    public void SetIsInActive(bool isInActive)
    {
        this.isInactive = isInActive;
    }
    public bool GetIsInActive()
    {
        return isInactive;
    }

    public void SetIsOnGround(bool isOnGround)
    {
        this.isOnGround = isOnGround;
    }

    public bool GetIsOnGround()
    {
        return isOnGround;
    }

    public void SetIsOnStair(bool isOnStair)
    {
        this.isOnStair = isOnStair;
    }

    public bool GetIsOnStair()
    {
        return isOnStair;
    }

    public void SetBlockForward(bool blockForward)
    {
        this.blockForward = blockForward;
    }

    public bool GetBlockForward()
    {
        return blockForward;
    }

    public void SetBlockDown(bool blockDown)
    {
        this.blockDown = blockDown;
    }

    public bool GetBlockDown()
    {
        return blockDown;
    }
}