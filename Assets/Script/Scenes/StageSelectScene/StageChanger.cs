using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChanger
{
    public void SetNextStage(int id)
    {
        StageData stage = GameManager.Data.Map.GetStage(id);

        GameManager.Data.Map.CurrentTileID = id;

        if (stage.Type == StageType.Tutorial)
        {
            SceneChanger.SceneChange("BattleScene");
        }
        else if (stage.Type == StageType.Battle)
        {
            SceneChanger.SceneChange("BattleScene");
        }
        else if (stage.Type == StageType.Store)
        {
            // NPC Å¸¶ô ÄÆ¾À Ã¼Å©
            bool isCutSceneOn = CutSceneChange();

            if (isCutSceneOn == false)
                SceneChanger.SceneChange("EventScene");
        }
    }

    private bool CutSceneChange()
    {
        bool isCutScene = false;
        StageName stageName = GameManager.Data.Map.GetCurrentStage().Name;

        if (stageName == StageName.Baptism)
        {
            if (!GameManager.OutGameData.Data.IsBaptismCorrupt && 
                GameManager.OutGameData.Data.BaptismCorruptValue >= 120 && GameManager.OutGameData.Data.PhanuelClear)
            {
                isCutScene = true;
                SceneChanger.SceneChangeToCutScene(CutSceneType.NPC_Baptism_Corrupt);
                GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_NPC_UPGRADE);
            }
        }
        else if (stageName == StageName.Stigmata)
        {
            if (!GameManager.OutGameData.Data.IsStigmataCorrupt && 
                GameManager.OutGameData.Data.StigmataCorruptValue >= 40 && GameManager.OutGameData.Data.YohrnClear)
            {
                isCutScene = true;
                SceneChanger.SceneChangeToCutScene(CutSceneType.NPC_Stigmata_Corrupt);
                GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_NPC_STIGMATA);
            }
        }
        else if (stageName == StageName.Sacrifice)
        {
            if (!GameManager.OutGameData.Data.IsSacrificeCorrupt && 
                GameManager.OutGameData.Data.SacrificeCorruptValue >= 80 && GameManager.OutGameData.Data.SaviorClear)
            {
                isCutScene = true;
                SceneChanger.SceneChangeToCutScene(CutSceneType.NPC_Sacrifice_Corrupt);
                GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_NPC_HARLOT);
            }
        }

        return isCutScene;
    }
}