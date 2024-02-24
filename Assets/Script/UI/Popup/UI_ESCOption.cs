using System;
using UnityEngine;

public class UI_ESCOption : UI_Popup
{
    // ��ư Hover ���� ����Ʈ �ʿ�?

    public void QuitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.OnOffESCOption();
    }

    public void OptionButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.ShowPopup<UI_Option>();
        GameManager.UI.IsCanESC = false;
    }

    public void GoToMainButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.SaveManager.DeleteSaveData();
        SceneChanger.SceneChange("MainScene");
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.SaveManager.SaveGame();
        Application.Quit();
    }
}
