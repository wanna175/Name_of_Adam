using System;
using UnityEngine;

public class UI_ESCOption : UI_Popup
{
    [SerializeField] private GameObject ConfirmToMain;

    private void Start()
    {
        ConfirmToMain.SetActive(false);
    }

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
        ConfirmToMain.SetActive(true);
    }

    public void YesGoToMain()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.SaveManager.DeleteSaveData();
        SceneChanger.SceneChange("MainScene");
        Time.timeScale = 1;
    }

    public void NoGoToMain()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        ConfirmToMain.SetActive(false);
    }

    public void ExitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        Application.Quit();
    }
}
