using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public enum SteamAchievementType
{
    None = 0,
    Test,
}

public struct SteamAchievementData
{
    public string achievementAPIKey;
    public string statAPIKey;
    public int curCount;
    public int maxCount;

    public SteamAchievementData(string achievementAPIKey, string statAPIKey, int curCount, int maxCount)
    {
        this.achievementAPIKey = achievementAPIKey;
        this.statAPIKey = statAPIKey;
        this.curCount = curCount;
        this.maxCount = maxCount;
    }
}

public class SteamClientManager : MonoBehaviour
{
    Dictionary<SteamAchievementType, SteamAchievementData> achievementDatas;

    public void Init()
    {
        achievementDatas = new Dictionary<SteamAchievementType, SteamAchievementData>()
        {
            { SteamAchievementType.None, new SteamAchievementData() },
            { SteamAchievementType.Test, new SteamAchievementData("TEST_ACHIEVEMENT", "TEST_STAT", 0, 1) },
        };
    }

    public void IncreaseAchievement(SteamAchievementType type)
    {
        if (!SteamManager.Initialized)
        {
            GameManager.Instance.SetSystemInfoText($"Steam is not connected.");
            return;
        }

        achievementDatas.TryGetValue(type, out SteamAchievementData data);

        if (string.IsNullOrEmpty(data.achievementAPIKey))
        {
            GameManager.Instance.SetSystemInfoText("No steam achievement is found.");
            return;
        }

        bool isSuccess = SteamUserStats.GetAchievement(data.achievementAPIKey, out bool achievementCompleted);
        if (isSuccess == false)
        {
            GameManager.Instance.SetSystemInfoText($"Failed to get steam achievement : {data.achievementAPIKey}");
            return;
        }

        if (achievementCompleted)
        {
            GameManager.Instance.SetSystemInfoText($"The steam acheivement is done : {data.achievementAPIKey}");
            return; // 이미 완료된 업적
        }

        SteamUserStats.GetStat(data.statAPIKey, out data.curCount);
        data.curCount++;
        SteamUserStats.SetStat(data.statAPIKey, data.curCount);
        SteamUserStats.StoreStats();

        GameManager.Instance.SetSystemInfoText($"스팀 업적 증가 : {data.achievementAPIKey}/{data.curCount}/{data.maxCount}");

        if (data.curCount >= data.maxCount) 
        {
            GameManager.Instance.SetSystemInfoText($"스팀 업적 달성! : {data.achievementAPIKey}/{data.curCount}/{data.maxCount}/{achievementCompleted}");
            SteamUserStats.SetAchievement(data.achievementAPIKey);
            SteamUserStats.StoreStats();
        }
    }
}
