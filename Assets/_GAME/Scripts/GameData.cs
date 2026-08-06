using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    [SerializeField] private AllLevelDataSave allLevelSaveData;

    [SerializeField] private PlayerData playerData;

    public List<GameObject> listDecorObject = new List<GameObject>();
    public ColorDataSO ColorDataSO;
    public List<LevelDataSO> LevelDatas = new List<LevelDataSO>();

    public List<SkinSO> SkinDatas = new List<SkinSO>();

    public AllLevelDataSave AllLevelSaveData => allLevelSaveData;

    public void SaveLevel(LevelDataSave levelData)
    {
        
        bool levelExist = false;
        for (int i = 0; i < allLevelSaveData.LevelDatas.Count; i++)
        {
            if (allLevelSaveData.LevelDatas[i].LevelId == levelData.LevelId)
            {
                levelExist = true;
                if (allLevelSaveData.LevelDatas[i].TotalStar < levelData.TotalStar)
                {
                    allLevelSaveData.LevelDatas[i] = levelData;
                }

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

    public void SavePlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
        string jsonSave = JsonUtility.ToJson(playerData);

        PlayerPrefs.SetString("playerData", jsonSave);

        PlayerPrefs.Save();
    }

    public void ChangeGoldPlayerData(int gold)
    {
        playerData.Gold += gold;
        SavePlayerData(playerData);
    }

    public int GetGold()
    {
        return playerData.Gold;
    }

    public void AddSkin(int skinId)
    {
        playerData.collectedSkin.Add(skinId);
        SavePlayerData(playerData);
    }
    public void SetCurrentSkin(int skinId)
    {
        playerData.CurrentSkinId = skinId;
        GameManager.Instance.GetPlayer().SetSkin(SkinDatas[skinId].SkinPrefab);
        SavePlayerData(playerData);
    }

    public int GetCurrentSkin()
    {
        return playerData.CurrentSkinId;
    }

    public List<int> GetCollectedSkin()
    {
        return playerData.collectedSkin;
    }

    public bool GetIsMute()
    {
        return playerData.IsMute;
    }

    public void SetIsMute(bool active)
    {
        playerData.IsMute = active;
        SavePlayerData(playerData);
        SoundManager.Instance.SetMuteSound(active);
    }
    public void SaveNamePlayerData(String name)
    {
        playerData.PlayerName = name;
        SavePlayerData(playerData);
    }

    public void LoadPlayerData()
    {
        string jsonPlayerData = PlayerPrefs.GetString("playerData");

        if (!string.IsNullOrEmpty(jsonPlayerData))
        {

        }
        else
        {
            SavePlayerData(new PlayerData
            {
                PlayerName = "Player1",
                collectedSkin = new List<int>(){0},
                CurrentSkinId = 0
            });
            jsonPlayerData = PlayerPrefs.GetString("playerData");
        }

        this.playerData = JsonUtility.FromJson<PlayerData>(jsonPlayerData);
        GameManager.Instance.GetPlayer().SetName(playerData.PlayerName);
        if (playerData.CurrentSkinId > -1 && playerData.CurrentSkinId < SkinDatas.Count)
        {
            GameManager.Instance.GetPlayer().SetSkin(SkinDatas[playerData.CurrentSkinId].SkinPrefab);
        }
    }

    [ContextMenu("CLEAR LEVELDATA")]
    public void ClearLevelData()
    {
        PlayerPrefs.DeleteKey("levelSave");
    }

    [ContextMenu("CLEAR PLAYERDATA")]
    public void ClearPlayerData()
    {
        PlayerPrefs.DeleteKey("playerData");
    }

    public void LoadLevelData()
    {
        string jsonLevelData = PlayerPrefs.GetString("levelSave");

        if (!string.IsNullOrEmpty(jsonLevelData))
        {
            allLevelSaveData = JsonUtility.FromJson<AllLevelDataSave>(jsonLevelData);

            Debug.Log("LoAd SUCCESSFUL");
        }
        else
        {
            allLevelSaveData = new AllLevelDataSave();
            allLevelSaveData.LevelDatas = new List<LevelDataSave>();
            for (int i = 0; i < LevelDatas.Count; i++)
            {
                allLevelSaveData.LevelDatas.Add(new LevelDataSave
                {
                    LevelId = LevelDatas[i].LevelId,
                    TotalStar = 0
                });
            }
            SaveAllLevel();

        }

    }


}
