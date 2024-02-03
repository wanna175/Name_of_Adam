using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void SceneChange(string scenename)
    {
        if (scenename != "ProgressShopScene" && SceneManager.GetActiveScene().name != "ProgressShopScene")
        {
            GameManager.Sound.SceneBGMPlay(scenename);
        }
        SceneManager.LoadScene(scenename);
    }
    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().name.ToString();
    }
}