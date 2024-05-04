using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public enum AchievementType
{
    None = 0,
    Test,
}

public class SteamClientManager : MonoBehaviour
{
    Dictionary<AchievementType, string> achievementKeys;

    public void Init()
    {
        achievementKeys = new Dictionary<AchievementType, string>()
        {
            { AchievementType.None, string.Empty },
            { AchievementType.Test, "TEST_ACHIEVEMENT_1_0" },
        };
    }

    public void IncreaseAchievement(AchievementType type)
    {
        if (!SteamManager.Initialized)
        {
            GameManager.Instance.SetSystemInfoText($"Steam is not connected.");
            return;
        }

        achievementKeys.TryGetValue(type, out string achievementKey);

        if (string.IsNullOrEmpty(achievementKey))
        {
            Debug.Log("No steam achievement is found.");
            return;
        }

        SteamUserStats.GetAchievement(achievementKey, out bool achievementCompleted);

        if (achievementCompleted)
        {
            GameManager.Instance.SetSystemInfoText($"스팀 업적 완료됨 : {achievementKey}");
            return; // 이미 완료된 업적
        }

        SteamUserStats.GetStat(achievementKey, out int curCount);
        SteamUserStats.SetStat(achievementKey, curCount + 1);
        SteamUserStats.StoreStats();

        SteamUserStats.GetAchievement(achievementKey, out achievementCompleted);

        GameManager.Instance.SetSystemInfoText($"스팀 업적 현황 : {achievementKey}/{curCount}/{achievementCompleted}");

        if (achievementCompleted) 
        {
            SteamUserStats.SetAchievement(achievementKey);
            SteamUserStats.StoreStats();
        }
    }
}
