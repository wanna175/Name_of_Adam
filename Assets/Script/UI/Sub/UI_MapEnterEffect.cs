using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UI_MapEnterEffect : MonoBehaviour
{
    public GameObject MapEnterEffect;

    private void Awake()
    {
        if(GameManager.Data.Map.CurrentTileID == 0)
        {
            GameManager.Sound.Play("Stage_Transition/StageSelectScene_Enter/StageSelectScene_Enter");
            MapEnterEffect.SetActive(true);
        }
        else
        {
            MapEnterEffect.SetActive(false);
        }
    }
}
