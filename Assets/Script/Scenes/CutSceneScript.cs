using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    private void Awake()
    {
        //GameManager.Sound.Play("");
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
