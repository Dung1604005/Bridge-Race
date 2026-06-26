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
        ColorType oldColor = colorType;
        colorType = _colorType;
        if (oldColor == ColorType.NONE)
        {
            renderer.enabled = true;
        }
        ChangeColorEffect(GameData.Instance.ColorDataSO.GetColorMaterial(oldColor).color, GameData.Instance.ColorDataSO.GetColorMaterial(_colorType).color, 0.5f);

        



    }

    public void ChangeColorEffect(Color startColor, Color targetColor, float duration)
    {
        if (changeColorCoroutine != null)
        {
            StopCoroutine(changeColorCoroutine);
        }

        changeColorCoroutine = StartCoroutine(IEChangeColorEffect(startColor, targetColor, duration));
    }

    private IEnumerator IEChangeColorEffect(Color startColor, Color targetColor, float duration)
    {

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float p = timer / duration;

            renderer.material.color = Color.Lerp(startColor, targetColor, p);

            yield return null;
        }
        renderer.material.color = targetColor;
    }

    public void TakeStair(Character character)
    {
        if (character.ColorType == colorType)
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
