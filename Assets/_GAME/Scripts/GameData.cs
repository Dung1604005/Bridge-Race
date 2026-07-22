using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public String ANIM_RUN = "Run";

    public String ANIM_IDLE = "Idle";

    public String ANIM_KNOCKBACK = "KnockBack";

    public String ANIM_WIN = "Win";

    public Vector3 BRICK_SIZE = new Vector3(1, 0.2f, 0.5f);

    public String CHARACTER_TAG = "Character";

    public int LAYER_STAIR = 6;

    public List<GameObject> listDecorObject = new List<GameObject>();
   [SerializeField] public ColorDataSO ColorDataSO ;

   public List<LevelDataSO> LevelDatas = new List<LevelDataSO>();

   public int TotalGold => totalGold;

   public AllLevelData AllLevelSaveData => allLevelSaveData;
   private int totalLevel;

   private int totalGold;

   [SerializeField] private String playerName;

   [SerializeField] private AllLevelData allLevelSaveData;



   public void SaveGold(int amountGold)
    {
        totalGold = amountGold;

        PlayerPrefs.SetInt("gold", totalGold);

        PlayerPrefs.Save();
    }
   public void SaveLevel(LevelData levelData)
    {
        bool levelExist = false;
        for(int i = 0; i < allLevelSaveData.LevelDatas.Count; i++)
        {
            if(allLevelSaveData.LevelDatas[i].LevelId == levelData.LevelId)
            {
                levelExist = true;
                allLevelSaveData.LevelDatas[i] = levelData;
            }
        }
        if (!levelExist)
        {
            allLevelSaveData.LevelDatas.Add(levelData);
        }
        SaveAllLevel();
    }
    

    public void SaveAllLevel()
    {
        string jsonSave = JsonUtility.ToJson(allLevelSaveData);

        PlayerPrefs.SetString("levelSave", jsonSave);

        PlayerPrefs.Save();

        Debug.Log("SAvE SUCCESSFUL");
    }

    public void SavePlayerData()
    {
        String playerName = GameManager.Instance.Player.CharacterName;

        PlayerData playerData = new PlayerData();

        playerData.PlayerName = playerName;
        string jsonSave = JsonUtility.ToJson(playerData);

        PlayerPrefs.SetString("namePlayer", jsonSave);

        PlayerPrefs.Save();


    }

    public void LoadPlayerData()
    {
        string jsonPlayerData = PlayerPrefs.GetString("namePlayer");

        if (!string.IsNullOrEmpty(jsonPlayerData))
        {
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonPlayerData);
            GameManager.Instance.Player.SetName(playerData.PlayerName);
        }
        else
        {
            Debug.Log("DONT HAVE PLAYERDATA");
        }
        
    }

    public void LoadData()
    {
        string jsonLevelData = PlayerPrefs.GetString("levelSave");

        if (!string.IsNullOrEmpty(jsonLevelData))
        {
            allLevelSaveData = JsonUtility.FromJson<AllLevelData>(jsonLevelData);

            Debug.Log("LoAd SUCCESSFUL");
        }
        else
        {
            allLevelSaveData = new AllLevelData();
            allLevelSaveData.LevelDatas = new List<LevelData>();
            SaveAllLevel();

        }
        totalGold = PlayerPrefs.GetInt("gold");

    }


}
