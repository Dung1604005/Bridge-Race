using UnityEngine;


public enum ColorType
{
    NONE = 0,
    RED = 1,
    BLUE = 2,

    YELLOW = 3,
    VIOLET = 4,
    BLACK = 5,
    GREEN = 6
}
[CreateAssetMenu(fileName = "ColorDataSO", menuName = "Scriptable Objects/ColorDataSO")]
public class ColorDataSO : ScriptableObject
{
    [SerializeField] private Material[] materials;

    public Material GetColorMaterial(ColorType colorType)
    {
        return materials[(int)colorType];
    }
}
