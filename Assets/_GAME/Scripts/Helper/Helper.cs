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
}
