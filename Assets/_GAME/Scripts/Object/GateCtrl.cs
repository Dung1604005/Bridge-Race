using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class GateCtrl : GameUnit
{
    [SerializeField] private LevelDataSO levelDataSO;
    [SerializeField] List<Renderer> gateObjRenderers = new List<Renderer>();

    [SerializeField] Transform transformDoor;

    [SerializeField] Stage nextStage;

    [SerializeField] float durationEffect;

    private bool taked;

    private Coroutine gateCoroutine;

    public Stage NextStage => nextStage;

    [ContextMenu("CREATE DATA")]
    public void CreateData()
    {
        GateData data = new GateData();

        data.TFData = Helper.CreateDataFromTransform(tf);

        if (nextStage == null)
        {
            data.NextStageNumber = -1;
        }
        else
        {
            data.NextStageNumber = nextStage.GetStageNumber();
        }


        levelDataSO.AddGateData(data);
        EditorUtility.SetDirty(levelDataSO);
        AssetDatabase.SaveAssets();
    }

    public void LoadData(GateData gateData)
    {
        Helper.LoadTransformData(tf, gateData.TFData);
        nextStage = LevelManager.Instance.StageManager.GetStage(gateData.NextStageNumber);
    }

    public void OnInit()
    {
        taked = false;
        nextStage = null;
    }

    public void OnDespawn()
    {
        taked = false;
        nextStage = null;
        for (int i = 0; i < gateObjRenderers.Count; i++)
        {
            gateObjRenderers[i].material = GameData.Instance.ColorDataSO.GetColorMaterial(ColorType.NONE);
        }
        transformDoor.localScale = Vector3.one;
    }

    public void OnColliderCharacter(Collider collider)
    {
        Character character = ColliderCache<Character>.GetComponent(collider);
        ColorType charColor = character.ColorType;

        if (!CanGoThroughGate(character)) return;

        character.ChangeStage(nextStage);

        if (gateCoroutine == null)
        {
            gateCoroutine = StartCoroutine(IEOpenDoor(durationEffect));
            
        }
        if (taked) return;
        taked = true;
        StartCoroutine(IEChangeColor(durationEffect, GameData.Instance.ColorDataSO.GetColorMaterial(charColor).color));
    }

    public void OnTriggerEnter(Collider collider)
    {

        if (collider.CompareTag(GameConfig.CHARACTER_TAG))
        {
            OnColliderCharacter(collider);
        }
    }

    public bool CanGoThroughGate(Character character)
    {
        if (nextStage == null) return true;

        if (character.CharacterState.GetIsInActive() || character.CharacterIsGoingDown() ||
        (character.CompareCurrentStage(nextStage) >= 0 && !character.IsBot))
        {
            return false;
        }
        return true;
    }

    IEnumerator IEOpenDoor(float duration)
    {
        float timer = 0f;
        Vector3 targetScale = new Vector3(0f, transformDoor.localScale.y, transformDoor.localScale.z);
        while (timer + 0.01f < duration)
        {
            timer += Time.deltaTime;
            transformDoor.localScale = Vector3.Lerp(transformDoor.localScale, targetScale, timer / duration);


            yield return null;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(IECloseDoor(duration));
    }

    IEnumerator IECloseDoor(float duration)
    {
        float timer = 0f;
        Vector3 targetScale = new Vector3(1f, transformDoor.localScale.y, transformDoor.localScale.z);
        while (timer + 0.01f < duration)
        {
            timer += Time.deltaTime;
            transformDoor.localScale = Vector3.Lerp(transformDoor.localScale, targetScale, timer / duration);


            yield return null;
        }

        gateCoroutine = null;
    }
    IEnumerator IEChangeColor(float duration, Color target)
    {
        float timer = 0f;

        while (timer + 0.01f < duration)
        {
            timer += Time.deltaTime;
            foreach (Renderer renderer in gateObjRenderers)
            {

                renderer.material.color = Color.Lerp(renderer.material.color, target, timer / duration);
            }
            yield return null;
        }

    }

    void Awake()
    {
        tf = transform;
    }
}

