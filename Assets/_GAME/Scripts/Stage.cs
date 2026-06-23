using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private float scaleX;

    [SerializeField] private float scaleY;

    [SerializeField] private float scaleZ;

    [SerializeField] private Vector3 sizeStage;

    [SerializeField] private List<Brick> bricks;

    private Transform tf;

    public void OnInit()
    {
        tf.localScale = new Vector3(scaleX, scaleY, scaleZ);

        //Vi size goc la 2 don vi nen *2
        sizeStage = (new Vector3(scaleX, scaleY, scaleZ))*2;

    }

    public  void SpawnBrick(List<ColorType> colorTypes)
    {
        int numbBrick = (int)(sizeStage.x/(GameData.Instance.BRICK_SIZE.x *2)) * (int)(sizeStage.z/(GameData.Instance.BRICK_SIZE.z *2)) ;

        //Moi stage se co 4 mau va spawn so luong brick cua tung loai mau giong nhau
        List<int> numbColorBrick = new List<int>();

        for(int i = 0; i < 4; i++)
        {
            numbColorBrick.Add(numbBrick/4);
        }
        
        
        Vector3 leftBottomPos = tf.position - new Vector3(sizeStage.x/2, tf.position.y + GameData.Instance.BRICK_SIZE.y/2, sizeStage.z/2);

        for(float x = leftBottomPos.x; x +0.01f < tf.position.x + sizeStage.x/2; x += 1)
        {
            for(float z = leftBottomPos.z; z + 0.01f < tf.position.z +  sizeStage.z/2 ; z+= 1)
            {
                
                SimplePool.Spawn<Brick>(PoolType.BrickPool, new Vector3(x, tf.position.y + GameData.Instance.BRICK_SIZE.y/2, z), Quaternion.identity);
            }
        }

        
    }

    void Awake()
    {
        tf = this.transform;
        OnInit();
    }
}
