using UnityEngine;

public class SwingingObject : MonoBehaviour
{
    [SerializeField] private float swingRange;

    [SerializeField] private float swingSpeed;

    [SerializeField] private Transform tf;

    private float currentRange = 0f;

    private float directionMulti = 1f;

    public void OnInit()
    {
        currentRange = 0f;
        tf = this.transform;
    }

    void Update()
    {
        if(GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }
        
        tf.eulerAngles = Vector3.MoveTowards(tf.eulerAngles, 
        new Vector3(tf.eulerAngles.x, tf.eulerAngles.y, directionMulti * swingRange), 
        swingSpeed*Time.deltaTime);
        
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
        if (collider.CompareTag(GameData.Instance.CHARACTER_TAG))
        {

            Character character = ColliderCache<Character>.GetComponent(collider);

            if (character.CharacterState.IsInactive) return;
            
            Vector3 knockbackDirAB = -tf.position + character.TF.position;
            knockbackDirAB.y = 1.8f;
            
            knockbackDirAB.Normalize();
            
            character.Knockback(knockbackDirAB);
                
                
                
           

        }
    }

    

}
