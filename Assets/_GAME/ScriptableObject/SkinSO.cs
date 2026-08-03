using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinSO", menuName = "Scriptable Objects/SkinSO")]
public class SkinSO : ScriptableObject
{

    [SerializeField] private int idSkin;

    [SerializeField] private String nameSkin;

    [SerializeField] private SkinController skinPrefab;

    [SerializeField] private GameObject modelPrefab;

    [SerializeField] private Sprite skinPortrait;

    [SerializeField] private int price;
    public int IdSkin => idSkin;
    public String NameSkin => nameSkin;
    public SkinController SkinPrefab => skinPrefab;

    public GameObject ModelPrefab => modelPrefab;
    public Sprite SkinPortrait => skinPortrait;

    public int Price => price;

}
