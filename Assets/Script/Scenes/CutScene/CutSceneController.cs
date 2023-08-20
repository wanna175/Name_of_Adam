using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    private void Start()
    {
        GameManager.Sound.Clear();
        //GameManager.Sound.Play("Stage_Transition/CutScene/CutSceneBGM");
    }

    public void SceneChange()
    {
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
