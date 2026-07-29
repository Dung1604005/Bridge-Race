using UnityEngine;

public class CharacterModelUI : MonoBehaviour
{
    [SerializeField] private Transform tf;
   [SerializeField] private Vector3 spawnPos;

   [SerializeField] private GameObject skinModel;


   public void OnEnable()
    {
        EventBus<OnLoadSkinModel>.Subcribe(OnChangeSkin);
    }

    public void OnDisable()
    {
        EventBus<OnLoadSkinModel>.UnSubcribe(OnChangeSkin);
    }

   public void OnChangeSkin(OnLoadSkinModel onLoadSkinModel)
    {
        Debug.Log(onLoadSkinModel.SkinId);
        SetSkin(GameData.Instance.SkinDatas[onLoadSkinModel.SkinId].ModelPrefab);
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
