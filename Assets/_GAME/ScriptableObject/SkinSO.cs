using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinSO", menuName = "Scriptable Objects/SkinSO")]
public class SkinSO : ScriptableObject
{
    public int IdSkin;
    public String NameSkin;
    public SkinController SkinPrefab;

    public GameObject ModelPrefab;
    public Sprite SkinPortrait;

    public int Price;

}
