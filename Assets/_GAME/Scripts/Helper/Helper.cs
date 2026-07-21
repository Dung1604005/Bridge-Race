using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Helper
{
    public static void LoadTransformData(Transform transform, TransformData data)
    {
        if(transform == null)
        {
            Debug.LogError("TRANSFORM NULL CANNOT LOAD DATA");
            return;
        }

        transform.localPosition = data.Position;

        transform.localRotation = Quaternion.Euler(data.EulerAngles);

        transform.localScale = data.Scale;
    }

    public static TransformData CreateDataFromTransform(Transform tf)
    {
        if(tf == null)
        {
            Debug.LogError("TRANSFORM NULL CANNOT READ");
            return new TransformData();
        }

        TransformData data = new TransformData();

        data.Position = tf.localPosition;

        data.EulerAngles = tf.eulerAngles;

        data.Scale = tf.localScale;

        return data;
    }

    public static Transform CreateTransformFromData(TransformData data, Transform parent = null)
    {
        GameObject emptyObj = new GameObject();

        emptyObj.transform.SetParent(parent, true);

        emptyObj.transform.localPosition = data.Position;

        emptyObj.transform.eulerAngles = data.EulerAngles;

         emptyObj.transform.localScale = data.Scale;

        return emptyObj.transform;
    }

    public static IEnumerator IEDoScale(Transform tf, Vector3 scale, float duration)
    {
        float elapseTime = 0f;

        while(elapseTime + 0.001f < duration)
        {
            elapseTime += Time.deltaTime;
            tf.localScale = Vector3.Lerp(tf.localScale, scale, elapseTime/duration);
            yield return null;
        }
    }

    public static IEnumerator IEDoScaleOutBack(Transform tf, Vector3 scale, float duration1, float duration2)
    {
        float elapseTime = 0f;

        Vector3 startScale = tf.localScale;

        while(elapseTime + 0.001f < duration1 + duration2)
        {
            elapseTime += Time.deltaTime;
            tf.localScale = Vector3.LerpUnclamped(startScale, scale, elapseTime/duration1);
            yield return null;
        }

        elapseTime = 0f;
        startScale = tf.localScale;
        while(elapseTime + 0.01f < duration2)
        {
            elapseTime += Time.deltaTime;
            tf.localScale = Vector3.Lerp(startScale, scale, elapseTime/duration2);
            yield return null;
        }

    }


    public static IEnumerator IEPopUp(RectTransform tf, Vector2 target ,float duration1, float duration2)
    {
        float timer = 0f;
        Vector2 startPos = tf.anchoredPosition;
        while(timer + 0.0001f < duration1 + duration2)
        {
            timer += Time.deltaTime;

            tf.anchoredPosition = Vector2.LerpUnclamped(startPos,target, timer/duration1 );
            yield return null;
        }

        timer = 0f;
        startPos = tf.anchoredPosition;

          while(timer + 0.0001f < duration2)
        {
            timer += Time.deltaTime;

            tf.anchoredPosition = Vector2.Lerp(startPos,target, timer/duration2 );
            yield return null;
        }


        
        
    }


}
