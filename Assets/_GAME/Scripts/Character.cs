using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Thong so
    [SerializeField] protected float speed;

    [SerializeField] private ColorType colorType;

    [SerializeField] private Renderer renderer;

    [SerializeField] protected Animator anim;

    [SerializeField] protected Rigidbody rb;

    protected Transform tf;

    private String currentAnim;

    private bool isMoving;

    public bool IsMoving => isMoving;

    private bool isDead;

    public bool IsDead => isDead;

    
    public void OnInit()
    {
        
        isMoving = false;
        anim.applyRootMotion = false;
        isDead = false;
    }
    public void OnDespawn()
    {
        isMoving = false;
        isDead = true;
    }

    public void SetColor(ColorType colorType)
    {
        if(renderer != null)
        {
            renderer.material = GameData.Instance.ColorDataSO.GetColorMaterial(colorType) ;
        }
        else
        {
            Debug.LogError("DONT HAVE COLOR MATERIAL");
        }
    }

    public void ChangeAnim(String newAnim)
    {
        
        anim.SetTrigger(newAnim);
        currentAnim = newAnim;
        
    }

    void Awake()
    {
        tf = this.transform;
    }
}
