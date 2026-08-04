using System;
using System.Collections;
using UnityEngine;

public class Stair : GameUnit
{
    [SerializeField] private Bridge bridge;
    [SerializeField] private Renderer renderer;

    [SerializeField] private ColorType colorType;

    [SerializeField] private float paintCooldown;

    private float lastPaintTime = -67f;

    private int stairId = -1;

    public ColorType ColorType => colorType;

    private Coroutine changeColorCoroutine;

    public Bridge Bridge => bridge;

    public void LoadData(StairData stairData)
    {
        Helper.LoadTransformData(tf, stairData.TFData);
    }
    public void OnInit()
    {
        stairId = -1;
        colorType = ColorType.NONE;
        lastPaintTime = -67f;
    }

    public void OnDeSpawn()
    {
        stairId = -1;
        bridge = null;
        renderer.enabled = false;
        colorType = ColorType.NONE;
        renderer.sharedMaterial = GameData.Instance.ColorDataSO.GetColorMaterial(ColorType.NONE);
    }

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
        renderer.sharedMaterial.color = targetColor;
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
        
        if (character.CharacterIsGoingDown() || character.CharacterState.GetIsInActive())
        {
            return;
        }
        if (character.ColorType == colorType)
        {
            return;
        }
        if (Time.time - lastPaintTime < paintCooldown)
        {
            return; 
        }

        if (character.BrickCharacterManager.GetAmountVisualBrick() > 0)
        {
            SoundManager.Instance.PlaySfx(AudioClipType.SFX_BUILD_BRIDGE);
            lastPaintTime = Time.time;
            SetColor(character.ColorType);
            character.BrickCharacterManager.RemoveBrick();

            if (character.IsBot)
            {
                Enemy enemy = character as Enemy;
                enemy.SetStairId(GetStarId());
                enemy.CaculateDestination();
            }
            
        }
    }
}
