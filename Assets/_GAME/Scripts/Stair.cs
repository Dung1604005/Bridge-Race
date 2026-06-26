using System;
using System.Collections;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private Renderer renderer;

    [SerializeField] private ColorType colorType;

    public ColorType ColorType => colorType;

    private Coroutine changeColorCoroutine;

    public void SetColor(ColorType _colorType)
    {
        if(colorType == ColorType.NONE)
        {
            ChangeColorEffect(GameData.Instance.ColorDataSO.GetColorMaterial(colorType).color, GameData.Instance.ColorDataSO.GetColorMaterial(_colorType).color, 0.5f);
        }
        else
        {
            renderer.material = GameData.Instance.ColorDataSO.GetColorMaterial(colorType);
        }
        colorType = _colorType;
        
        

    }

    public void ChangeColorEffect(Color startColor, Color targetColor, float duration)
    {
        if(changeColorCoroutine != null)
        {
            StopCoroutine(changeColorCoroutine);
        }

        changeColorCoroutine = StartCoroutine(IEChangeColorEffect(startColor, targetColor, duration));
    }

    private IEnumerator IEChangeColorEffect(Color startColor, Color targetColor, float duration)
    {
        
        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;

            float p = timer/duration;

            renderer.material.color = Color.Lerp(startColor, targetColor, p);

            yield return null;
        }
        renderer.material.color = targetColor;
    }

    public void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Va cham stair");
        if (collider.CompareTag(GameData.Instance.CHARACTER_TAG))
        {
            
            Character character = ColliderCache<Character>.GetComponent(collider);
            if (character == null)
            {
                character = collider.gameObject.GetComponent<Character>();
                ColliderCache<Character>.AddComponent(collider, character);
            }

            if(character.ColorType == colorType)
            {
                return;
            }

            
            if (character.GetAmountBrick() > 0)
            {
                character.RemoveBrick();
                SetColor(character.ColorType);
            }
            
        }
    }
    


}
