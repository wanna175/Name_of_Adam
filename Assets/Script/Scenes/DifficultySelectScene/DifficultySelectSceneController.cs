using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelectSceneController : MonoBehaviour
{
    [SerializeField] GameObject UI_Difficulty;
    [SerializeField] GameObject UI_ChampionSelect;
    [SerializeField] GameObject UI_PlayerSkillSelect;
    [SerializeField] GameObject UI_HallSelect;


    private bool DifficultySelected;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        UI_Difficulty.SetActive(false);//추후 순서대로 수정
        UI_ChampionSelect.SetActive(false);
        UI_PlayerSkillSelect.SetActive(false);
        UI_HallSelect.SetActive(true);
        DifficultySelected = false;

    }

    public void Confirm()
    {
        SceneChanger.SceneChange("StageSelectScene");
        /*
        if(!DifficultySelected)
        {
            UI_Difficulty.SetActive(false);
            UI_ChampionSelect.SetActive(true);
            DifficultySelected=true;
        }
        else
        {

        }
        */
    }

    public void Quit()
    {
        SceneChanger.SceneChange("HubScene");
    }
}
