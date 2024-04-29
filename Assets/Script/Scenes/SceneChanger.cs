using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void SceneChange(string scenename)
    {
        if (scenename == "ProgressShopScene" || SceneManager.GetActiveScene().name == "ProgressShopScene")
        {
            SceneManager.LoadScene(scenename);
            return;
        }

        SceneManager.LoadScene(scenename);
        GameManager.VisualEffect.ClearAllEffect();
        GameManager.Sound.SceneBGMPlay(scenename);
    }

    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().name.ToString();
    }

    public static void SceneChangeToCutScene(CutSceneType cutSceneType)
    {
        Time.timeScale = 1.0f;
        GameManager.Data.CutSceneToDisplay = cutSceneType;
        GameManager.OutGameData.SetCutSceneData(cutSceneType, true);
        GameManager.OutGameData.SaveData();

        SceneChange("CutScene");
    }
}