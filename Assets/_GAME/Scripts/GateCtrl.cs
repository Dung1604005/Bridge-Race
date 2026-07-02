using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GateCtrl : MonoBehaviour
{
    [SerializeField] List<Renderer> gateObjRenderers = new List<Renderer>();

    [SerializeField] Transform transformDoor;

    [SerializeField] Stage nextStage;

    [SerializeField] float durationEffect;

    private Transform tf;

    private bool opened;

    public Stage NextStage => nextStage;

    public void OnTriggerEnter(Collider collider)
    {

        if (collider.CompareTag(GameData.Instance.CHARACTER_TAG))
        {
            Character character = ColliderCache<Character>.GetComponent(collider);
            ColorType charColor = character.ColorType;

            if(!CanGoThroughGate(character))return;
            
            character.ChangeStage(nextStage);
            if(opened)return;
            opened = true;
            StartCoroutine(IEOpenDoor(durationEffect));
            StartCoroutine(IEChangeColor(durationEffect, GameData.Instance.ColorDataSO.GetColorMaterial(charColor).color));
        }
    }

    public bool CanGoThroughGate(Character character)
    {
        if (nextStage == null) return true;

        if (character.IsInActive || character.CharacterIsGoingDown() ||
        (character.CurrentStage.StageNumber >= nextStage.StageNumber && !character.IsBot))
        {
            return false;
        }
        return true;
    }

    IEnumerator IEOpenDoor(float duration)
    {
        float timer = 0f;
        Vector3 targetScale = new Vector3(0f, tf.localScale.y, tf.localScale.z);
        while (timer + 0.01f < duration)
        {
            timer += Time.deltaTime;
            transformDoor.localScale = Vector3.Lerp(tf.localScale, targetScale, timer / duration);


            yield return null;
        }
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
