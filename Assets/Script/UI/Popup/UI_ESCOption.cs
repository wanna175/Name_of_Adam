using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ESCOption : UI_Popup
{
    public void ContinueButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        GameManager.UI.OnOffESCOption();
    }

    public void SettingButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        var option = GameManager.UI.ShowPopup<UI_Option>();
        option.GetComponent<Canvas>().sortingOrder = UIManager.ESCOrder + 1;
        GameManager.UI.ESCOPopups.Push(option);
    }

    public void MainMenuButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "BattleScene" && sceneName != "EventScene" && sceneName != "CutScene" 
            && sceneName != "ActSelectScene" && sceneName != "DifficultySelectScene")
        {
            GameManager.SaveManager.SaveGame();
        }

        Time.timeScale = 1;
        GameManager.VisualEffect.ClearAllEffect();
        GameManager.UI.CloseAllOption();

        SceneChanger.SceneChange("MainScene");
    }

    public void QuitButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        Application.Quit();
    }

    public void ButtonHover()
    {
        GameManager.Sound.Play("UI/UISFX/UIHoverSFX");
    }

    public void Quit2Button()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        GameManager.UI.OnOffESCOption();
    }

    public void OptionButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        var option = GameManager.UI.ShowPopup<UI_Option>();
        option.GetComponent<Canvas>().sortingOrder = UIManager.ESCOrder + 1;
        GameManager.UI.ESCOPopups.Push(option);
    }

    public void GoToMainButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "BattleScene" && sceneName != "EventScene" && sceneName != "CutScene")
        {
            GameManager.SaveManager.SaveGame();
        }

        Time.timeScale = 1;
        GameManager.VisualEffect.ClearAllEffect();
        GameManager.UI.CloseAllOption();

        SceneChanger.SceneChange("MainScene");
    }

    public void ExitButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        Application.Quit();
    }
}
