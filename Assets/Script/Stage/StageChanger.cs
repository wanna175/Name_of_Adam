using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageChanger
{
    // 다음 스테이지 이동용
    public static void SetNextStage(Stage stage)
    {
        Debug.Log("Now Stage : " + stage.Name);

        if (stage.GetStageType() == StageType.Battle)
        {
            // SceneChanger.SceneChange("JS TEST");
            SceneChanger.SceneChange("Battle");
        }
        else
            SceneChanger.SceneChange("EventScene");
    }

    private static void SetBattleScene(Stage stage)
    {
        
    }
}
