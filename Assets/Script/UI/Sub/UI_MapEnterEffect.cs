using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UI_MapEnterEffect : MonoBehaviour
{
    public GameObject MapEnterEffect;
    public GameObject TutorialFog;

    private void Awake()
    {
        if(GameManager.Data.Map.CurrentTileID == 0)
        {
            GameManager.Sound.Play("Stage_Transition/StageSelectScene_Enter/StageSelectScene_Enter");
            MapEnterEffect.SetActive(true);
        }
        else if (GameManager.Data.Map.GetCurrentStage().Type == StageType.Tutorial && GameManager.Data.Map.CurrentTileID == 3)
        {
            GameManager.Sound.Play("Stage_Transition/StageSelectScene_Enter/StageSelectScene_Enter");
            MapEnterEffect.SetActive(true);
        }
        else
        {
            MapEnterEffect.SetActive(false);
        }

        if (!GameManager.OutGameData.IsTutorialClear())
        {
            TutorialFog.SetActive(true);
        }
    }
}
