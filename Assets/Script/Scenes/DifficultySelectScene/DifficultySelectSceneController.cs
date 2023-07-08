using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelectSceneController : MonoBehaviour
{
    [SerializeField] GameObject UI_Difficulty;
    [SerializeField] GameObject UI_ChampionSelect;
    [SerializeField] GameObject UI_PlayerSkillSelect;


    private bool DifficultySelected;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        UI_Difficulty.SetActive(true);
        UI_ChampionSelect.SetActive(false);
        UI_PlayerSkillSelect.SetActive(false);
        DifficultySelected = false;

    }

    public void Confirm()
    {
        if(!DifficultySelected)
        {
            UI_Difficulty.SetActive(false);
            UI_ChampionSelect.SetActive(true);
            DifficultySelected=true;
        }
        else
        {

        }
    }

    public void Quit()
    {
        SceneChanger.SceneChange("HubScene");
    }
}
