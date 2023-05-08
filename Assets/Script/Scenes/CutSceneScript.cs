using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    private void Update()
    {
        if(sr.color.r <=0)
        {
            SceneChanger.SceneChange("StageSelectScene");
        }
    }

    
}
