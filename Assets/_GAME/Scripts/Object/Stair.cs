using System;
using System.Collections;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private Bridge bridge;
    [SerializeField] private Renderer renderer;

    [SerializeField] private ColorType colorType;

    private int stairId = -1;

    public ColorType ColorType => colorType;

    private Coroutine changeColorCoroutine;

    public Bridge Bridge => bridge;

    public void SetBridge(Bridge _bridge)
    {
        bridge = _bridge;
    }

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

    public bool IsThisLastStair()
    {
        if (bridge.Stairs[bridge.Stairs.Count - 1] == this)
        {
            return true;
        }
        return false;
    }

    public int GetStarId()
    {
        if(stairId == -1)
        {
            stairId = bridge.GetStairId(this);
            if(stairId == -1)
            {
                Debug.Log("DONT FIND ANY STAIR IN BRIDGE");
            }
            return stairId;
        }
        else
        {
            return stairId;
        }
    }

    public void TakeStair(Character character)
    {
        if (character.CharacterIsGoingDown() || character.IsInActive)
        {
            return;
        }
        if (character.ColorType == colorType)
        {
            return;
        }

        if (character.GetAmountBrick() > 0)
        {
            character.RemoveBrick();
            SetColor(character.ColorType);

            EventBus<OnStairChange>.Raise(new OnStairChange
            {
                CharacterId = character.CharacterId,
                StairId = GetStarId()
            });
        }
    }

   




}
