using System;
using UnityEngine;

public class UI_ESCOption : UI_Popup
{
    public void QuitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.OnOffESCOption();
    }

    public void OptionButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.ShowPopup<UI_Option>().GetComponent<Canvas>().sortingOrder = UIManager.ESCOrder + 1;
        GameManager.UI.IsCanESC = false;
    }

    public void GoToMainButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_SystemSelect systemSelect = GameManager.UI.ShowPopup<UI_SystemSelect>();
        systemSelect.GetComponent<Canvas>().sortingOrder = UIManager.ESCOrder + 1;
        systemSelect.Init("Restart", YesGoToMain);
    }

    public void YesGoToMain()
    {
        GameManager.SaveManager.DeleteSaveData();
        SceneChanger.SceneChange("MainScene");
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        Application.Quit();
    }
}
