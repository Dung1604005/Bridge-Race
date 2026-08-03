using UnityEngine;

public class SwingingObject : MonoBehaviour
{
    [SerializeField] private float swingRange;

    [SerializeField] private float swingSpeed;

    [SerializeField] private Transform tf;

    private float timePassed = 0f;

    private float directionMulti = 1f;

    public void OnInit()
    {
        timePassed = 0f;
        tf = this.transform;
    }

    void Update()
    {
        if(GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }
        timePassed += Time.deltaTime*swingSpeed;

        float currentAngle = Mathf.Sin(timePassed) * swingRange;
        tf.localRotation = Quaternion.Euler(tf.localEulerAngles.x, tf.localEulerAngles.y, currentAngle);
        
        if(Mathf.Abs(Mathf.Abs(tf.eulerAngles.z) - Mathf.Abs(swingRange)) <= 0.01f)
        {
            directionMulti *= -1;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }

        Collider collider = collision.collider;
        if (collider.CompareTag(GameConfig.CHARACTER_TAG))
        {

            Character character = ColliderCache<Character>.GetComponent(collider);

            if (character.CharacterState.GetIsInActive()) return;

            Vector3 impactPoint = collision.GetContact(0).point;
            
            Vector3 knockbackDir = (-character.TF.position + impactPoint);
            knockbackDir.y += 1.5f;
            
            
            
            character.Knockback(knockbackDir);
                
        }
    }

    

}
