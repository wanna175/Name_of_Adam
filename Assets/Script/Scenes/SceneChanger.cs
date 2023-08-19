using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void SceneChange(string scenename)
    {
        GameManager.VisualEffect.StartFadeEffect(false);

        GameManager.Instance.PlayAfterCoroutine(() =>
        {
            GameManager.Sound.SceneBGMPlay(scenename);
            SceneManager.LoadScene(scenename);
        }, 1);

    }
    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().ToString();
    }
}