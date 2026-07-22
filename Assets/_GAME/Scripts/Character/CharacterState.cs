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
}