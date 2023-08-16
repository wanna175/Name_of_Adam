using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void SceneChange(string scenename)
    {
        GameManager.Sound.SceneBGMPlay(scenename);
        SceneManager.LoadScene(scenename);
    }
    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().ToString();
    }
}