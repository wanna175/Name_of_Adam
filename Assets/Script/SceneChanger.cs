using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    
    public static void SceneChange(string SceneName)
    {
        
        if(SceneName == "Battle")
        {
            GameManager.Sound.Clear();
            GameManager.Sound.Play("Stage_Transition/Stage_Enter/Stage_EnterSFX");
            if(GameManager.Instance.tutorial_Trigger)
            {
                for(int i = 0; i <30; i++)
                {
                    GameManager.Resource.Load<GameObject>("Prefabs/UI/Popup/Tutorial/UI_Tutorial" + i);
                }
                
                GameManager.Instance.tutorial_Trigger = true;
            }
        }

        if(SceneName != "LogoScene")
        {
            GameManager.Sound.Clear();
            GameManager.Sound.Play(SceneName + "/" + SceneName + "BGM", Sounds.BGM);
        }
        
        SceneManager.LoadScene(SceneName);
    }
    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().ToString();
    }

}