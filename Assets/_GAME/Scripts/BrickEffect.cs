using UnityEngine;

public class BrickEffect : GameUnit
{
    [SerializeField] private ParticleSystem effect;

    [SerializeField] private ParticleSystemRenderer effectRenderer;

    public void Play()
    {
        effect.Play();

        Invoke(nameof(OnDespawn), effect.main.duration);


    }

    public void SetLocal(Vector3 position, Quaternion quaternion, Transform parent = null)
    {
        if(parent != null)
        {
            tf.SetParent(parent, true);
        }

        tf.localPosition = position;
        tf.localRotation = quaternion;
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
    public void SetColor(ColorType colorType)
    {
        effectRenderer.material = GameData.Instance.ColorDataSO.GetColorParticalMaterial(colorType);
    }

    void Awake()
    {
        this.tf = transform;
    }
}
