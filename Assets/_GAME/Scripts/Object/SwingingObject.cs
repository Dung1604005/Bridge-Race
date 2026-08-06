using UnityEngine;

public class SwingingObject : MonoBehaviour
{
    [SerializeField] private float swingRange;

    [SerializeField] private float swingSpeed;

    [SerializeField] private Transform tf;

    [SerializeField] private Transform impactPoint;

    private float timePassed = 0f;

    private float directionMulti = 1f;

    public void OnInit()
    {
        timePassed = 0f;
        tf = this.transform;
    }

    public void UpdateRotation()
    {
        timePassed += Time.deltaTime * swingSpeed;

        float currentAngle = Mathf.Sin(timePassed) * swingRange;
        tf.localRotation = Quaternion.Euler(tf.localEulerAngles.x, tf.localEulerAngles.y, currentAngle);

        if (Mathf.Abs(Mathf.Abs(tf.eulerAngles.z) - Mathf.Abs(swingRange)) <= 0.01f)
        {
            directionMulti *= -1;
        }
    }

    public void OnColliderPlayer(Collider collider)
    {
        Character character = ColliderCache<Character>.GetComponent(collider);

        if (character.CharacterState.GetIsInActive()) return;

        Vector3 knockbackDir = (character.TF.position - impactPoint.position);
        
        character.Knockback(knockbackDir);
    }

    void Update()
    {
        if (GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }
        UpdateRotation();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (GameManager.Instance.GameState != GameState.PLAYING)
        {
            return;
        }

        
        if (collider.CompareTag(GameConfig.CHARACTER_TAG))
        {
            OnColliderPlayer(collider);

        }
    }



}
