using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChanger
{
    // 다음 스테이지 이동용
    public void SetNextStage(Stage stage)
    {
        Debug.Log("Now Stage : " + stage.Name);

        if (stage.GetStageType() == "Battle")
        {
            // SceneChanger.SceneChange("JS TEST");
            SceneChanger.SceneChange("Battle");
        }
        else
            SceneChanger.SceneChange("EventScene");
    }

    private void SetBattleScene(Stage stage)
    {
        
    }
}
