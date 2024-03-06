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
                && GameManager.OutGameData.GetNPCQuest().UpgradeQuest >= 100)
            {
                isCutScene = true;
                GameManager.OutGameData.SetCutSceneData(CutSceneType.NPC_Upgrade_Corrupt, true);
                SceneChanger.SceneChangeToCutScene(CutSceneType.NPC_Upgrade_Corrupt);
            }
        }
        else if (name == "StigmaStore")
        {
            if (GameManager.OutGameData.GetCutSceneData(CutSceneType.NPC_Stigma_Corrupt) == false
                && GameManager.OutGameData.GetNPCQuest().StigmaQuest >= 50)
            {
                isCutScene = true;
                GameManager.OutGameData.SetCutSceneData(CutSceneType.NPC_Stigma_Corrupt, true);
                SceneChanger.SceneChangeToCutScene(CutSceneType.NPC_Stigma_Corrupt);
            }
        }
        else if (name == "Harlot")
        {
            if (GameManager.OutGameData.GetCutSceneData(CutSceneType.NPC_Harlot_Corrupt) == false
                && GameManager.OutGameData.GetNPCQuest().DarkshopQuest >= 30)
            {
                isCutScene = true;
                GameManager.OutGameData.SetCutSceneData(CutSceneType.NPC_Harlot_Corrupt, true);
                SceneChanger.SceneChangeToCutScene(CutSceneType.NPC_Harlot_Corrupt);
            }
        }

        return isCutScene;
    }
}