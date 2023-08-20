using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageChanger
{
    public void SetNextStage(int _id)
    {
        StageData stage = GameManager.Data.Map.StageList.Find(x => x.ID == _id);

        GameManager.Data.Map.CurrentTileID = _id;


        if(_id == 1)
        {
            GameManager.Data.MainDeckSet();
            SceneChanger.SceneChange("BattleScene");
        }
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
            SceneChanger.SceneChange("EventScene");
        }
    }
}