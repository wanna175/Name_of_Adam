using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCreditSceneController : MonoBehaviour
{
    private void Start()
    {
        GameManager.Sound.Clear();
        //GameManager.Sound.Play("Stage_Transition/CutScene/CutSceneBGM");
    }
    public void SceneChange()
    {
        SceneChanger.SceneChange("MainScene");
    }
}
