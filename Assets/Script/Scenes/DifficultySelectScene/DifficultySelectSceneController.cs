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
        if (UI_HallSelect)
        {
            GameManager.Data.MainDeckSet();
            GameManager.Data.GameData.FallenUnits.Clear();
            GameManager.Data.GameData.FallenUnits.AddRange(GameManager.Data.GameDataMain.DeckUnits);
        }

        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        SceneChanger.SceneChange("StageSelectScene");
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        SceneChanger.SceneChange("MainScene");
    }
}
