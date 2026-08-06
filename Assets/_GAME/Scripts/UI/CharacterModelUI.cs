using UnityEngine;

public class CharacterModelUI : MonoBehaviour
{
    [SerializeField] private Transform tf;
   [SerializeField] private Vector3 spawnPos;

   [SerializeField] private GameObject skinModel;

   [SerializeField] private GameObject cameraModel;

   public void OnChangeSkin(int skinId)
    {
        
        SetSkin(GameData.Instance.SkinDatas[skinId].ModelPrefab);
    }

    public void SetActiveCameraModel(bool active)
    {
        
        cameraModel.SetActive(active);
    }
   public void SetSkin(GameObject skinPrefab)
    {
        if(skinModel != null)
        {
            Destroy(skinModel);
        }
        GameObject skinController = GameObject.Instantiate(skinPrefab,spawnPos + tf.position, Quaternion.identity, tf );
        skinModel = skinController;
        
    }
}
