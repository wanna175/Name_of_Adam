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
        else if (stage.Type == StageType.BattleTest)
        {
            SceneChanger.SceneChange("BattleTestScene");
        }
    }

    private bool CutSceneChange()
    {
        bool isCutScene = false;
        string name = GameManager.Data.Map.GetCurrentStage().Name.ToString();

        if (name == "UpgradeStore")
        {
            if (GameManager.OutGameData.GetCutSceneData(CutSceneType.NPC_Upgrade_Corrupt) == false
                && GameManager.OutGameData.GetNPCQuest().UpgradeQuest >= 300)
            {
                isCutScene = true;
                SceneChanger.SceneChangeToCutScene(CutSceneType.NPC_Upgrade_Corrupt);
                GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_NPC_UPGRADE);
            }
        }
        else if (name == "StigmaStore")
        {
            if (GameManager.OutGameData.GetCutSceneData(CutSceneType.NPC_Stigma_Corrupt) == false
                && GameManager.OutGameData.GetNPCQuest().StigmaQuest >= 30)
            {
                isCutScene = true;
                SceneChanger.SceneChangeToCutScene(CutSceneType.NPC_Stigma_Corrupt);
                GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_NPC_STIGMATA);
            }
        }
        else if (name == "Harlot")
        {
            if (GameManager.OutGameData.GetCutSceneData(CutSceneType.NPC_Harlot_Corrupt) == false
                && GameManager.OutGameData.GetNPCQuest().DarkshopQuest >= 50)
            {
                isCutScene = true;
                SceneChanger.SceneChangeToCutScene(CutSceneType.NPC_Harlot_Corrupt);
                GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_NPC_HARLOT);
            }
        }

        return isCutScene;
    }
}