using UnityEngine;

public class CharacterKnockBack : MonoBehaviour
{
    [SerializeField] private Character character;

    [SerializeField] private float knockForce;

    [SerializeField] protected ParticleSystem breakBrickEffect;


    public void Knockback(Vector3 knockbackDirection)
    {

        character.SetInActive();
        //gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
        character.Rb.AddForce(knockbackDirection * knockForce, ForceMode.Impulse);
        character.TF.rotation = Quaternion.LookRotation(-knockbackDirection);
        if (character.BrickCharacterManager.GetAmountRealBrick() > 0)
        {
            breakBrickEffect.transform.position = character.BrickCharacterManager.GetBrickPosition(character.BrickCharacterManager.VisualBrickId / 2);
            breakBrickEffect.Play();
        }
        character.BrickCharacterManager.ClearBrick();
        character.ChangeAnim(GameData.Instance.ANIM_KNOCKBACK);

        EventBus<OnCharacterInActive>.Raise(new OnCharacterInActive
        {
            CharacterId = character.CharacterId
        });

    }

    public void StandUp()
    {
        character.CharacterState.SetIsInActive(false);

        character.CharacterState.SetBlockForward(false);

        character.CharacterState.SetBlockDown(false); 
    }


    public void OnCollisionEnter(Collision collision)
    {
        if(character.CharacterState.IsOnStair)return;

        if (character.CharacterState.IsInactive)
        {
            return;
        }
        
        Collider collider = collision.collider;
        if (collider.CompareTag(GameData.Instance.CHARACTER_TAG))
        {

            Character other = ColliderCache<Character>.GetComponent(collider);

            if (other.CharacterState.IsInactive) return;
            if(other.CharacterState.IsOnStair)return;

            Vector3 knockbackDirBA = other.TF.position - character.TF.position;
            Vector3 knockbackDirAB = character.TF.position - other.TF.position;
            knockbackDirAB.y = 0.8f;
            knockbackDirBA.y = 0.8f;
            knockbackDirAB.Normalize();
            knockbackDirBA.Normalize();
            if (other.BrickCharacterManager.GetAmountVisualBrick() < character.BrickCharacterManager.GetAmountVisualBrick())
            {
                other.Knockback(knockbackDirBA);
                character.SetInActive(0.1f);


            }
            else if (other.BrickCharacterManager.GetAmountVisualBrick() > character.BrickCharacterManager.GetAmountVisualBrick())
            {
                character.Knockback(knockbackDirAB);
                other.SetInActive(0.1f);

            }
            else
            {
                character.Knockback(knockbackDirAB);
                other.Knockback(knockbackDirBA);
            }

        }
    }
}
