using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static List<SpawnData> SpawnDataList = new List<SpawnData>();


    public static void SceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}