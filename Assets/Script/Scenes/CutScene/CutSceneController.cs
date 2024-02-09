using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    private void Start()
    {
        GameManager.Sound.Clear();
        GameManager.Sound.Play("UI/ClickSFX/UIClick2");
        //GameManager.Sound.Play("Stage_Transition/CutScene/CutSceneBGM");
    }

    public void SceneChange()
    {
        SceneChanger.SceneChange("StageSelectScene");
    }
    public void SkipButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        SceneChanger.SceneChange("StageSelectScene");
    }


    private void Update()
    {
        if(sr.color.r <=0)
        {
            SceneChange();
        }
    }

    
}
