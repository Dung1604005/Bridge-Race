using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Renderer meshRenderer;

    [SerializeField] ColorDataSO colorDataSO;

    private ColorType colorType;

    public ColorType ColorType => colorType;

    void Start()
    {
        ChangeColor(ColorType.YELLOW);
    }
    public void ChangeColor(ColorType colorType)
    {
        meshRenderer.material = colorDataSO.GetColorMaterial(colorType);
    }
}
