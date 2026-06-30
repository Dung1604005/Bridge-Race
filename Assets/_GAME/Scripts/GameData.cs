using System;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public String ANIM_RUN = "Run";

    public String ANIM_IDLE = "Idle";

    public String ANIM_KNOCKBACK = "KnockBack";

    public Vector3 BRICK_SIZE = new Vector3(1, 0.2f, 0.5f);

    public String CHARACTER_TAG = "Character";

    public int LAYER_STAIR = 1<<6;
   [SerializeField] private ColorDataSO colorDataSO;

   public ColorDataSO ColorDataSO => colorDataSO;


}
