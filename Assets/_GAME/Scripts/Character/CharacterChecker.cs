using UnityEngine;

public class CharacterChecker : MonoBehaviour
{

    [SerializeField] private Character character;

    [SerializeField] private Collider collider;

    [SerializeField] protected float rangeDetect;

    private int layerGround;

    private int layerStair;

    private LayerMask layerStairGround;

    private int layerGate;

    public void OnInit()
    {
        layerGround = LayerMask.GetMask("Ground", "Stair");
        layerStair = LayerMask.GetMask("Stair");
        layerGate = LayerMask.GetMask("Gate");
        layerStairGround = LayerMask.GetMask("StairGround");
    }
    public bool CharacterIsGoingDown()
    {

        if (character.Rb.linearVelocity.z < -0.01f)
        {
            return true;
        }
        return false;
    }

    public virtual bool CharacterIsFalling()
    {

        if (character.Rb.linearVelocity.y < -5f && !Physics.Raycast(character.TF.position, -character.TF.up, 10f, layerGround | layerStairGround))
        {
            character.CharacterState.SetIsOnGround(false);

            return true;
        }
        character.CharacterState.SetIsOnGround(true);
        return false;
    }

    public void SetCharacterOnStair(bool val)
    {
        character.CharacterState.SetIsOnStair(val);
        if (character.IsBot) collider.enabled = !val;
    }
    public void CheckGate()
    {
        if (Physics.Raycast(character.TF.position, character.TF.forward, out RaycastHit hitGate, rangeDetect, layerGate))
        {
            Collider col = hitGate.collider;
            GateCtrl gate = ColliderCache<GateCtrl>.GetComponent(col);

            //Cho di qua cong neu stage hien tai = stage tiep theo cua gate
            if (gate.NextStage == null || character.CompareCurrentStage(gate.NextStage) == 0)
            {
                character.CharacterState.SetBlockDown(true);

            }
            else
            {
                character.CharacterState.SetBlockDown(false);

            }
        }
        else
        {
            character.CharacterState.SetBlockDown(false);
        }
    }

    public void CheckStair()
    {

        if (character.CharacterIsGoingDown()) return;

        Debug.DrawRay(character.TF.position, character.TF.forward * rangeDetect);
        if (Physics.Raycast(character.TF.position, character.TF.forward, out RaycastHit hit, rangeDetect, layerStair))
        {

            Collider col = hit.collider;


            Stair stair = ColliderCache<Stair>.GetComponent(col);


            stair.TakeStair(character);
            // Neu current stage bang voi stage owener cua stair thi kiem tra
            if (character.CompareCurrentStage(stair.Bridge.OwnerStage) == 0)
            {
                // Cung mau thi pass khac thi nhot
                if (stair.ColorType == character.ColorType)
                {
                    character.CharacterState.SetBlockForward(false);
                }
                else
                {
                    character.CharacterState.SetBlockForward(true);
                }
            }
        }
        else
        {
            collider.enabled = true;
            character.CharacterState.SetIsOnStair(false);
            character.CharacterState.SetBlockForward(false);
        }
    }

    public void CheckForward()
    {

        //Check Gate
        CheckGate();
        //Check stair
        CheckStair();

    }
}
