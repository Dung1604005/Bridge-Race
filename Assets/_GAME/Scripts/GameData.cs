using System;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public String ANIM_RUN = "Run";

    public String ANIM_IDLE = "Idle";

    public Vector3 BRICK_SIZE = new Vector3(1, 0.2f, 0.5f);
   [SerializeField] private ColorDataSO colorDataSO;

   public ColorDataSO ColorDataSO => colorDataSO;


}
