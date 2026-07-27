using UnityEngine;

public class SkinController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private Renderer skinRenderer;

    [SerializeField] private Transform brickRoot;


    public Animator Anim => anim;

    public Renderer SkinRenderer => skinRenderer;

    public Transform BrickRoot=> brickRoot;
}
