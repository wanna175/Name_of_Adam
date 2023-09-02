using System;
using UnityEngine;

public class UI_ESCOption : UI_Popup
{
    public void QuitButton()
    {
        GameManager.UI.ClosePopup();
        GameManager.Instance.escOption = false;
        if (GameManager.Instance.isTutorialactive)
        {
            return;
        }

        Time.timeScale = 1;
    }
    public void OptionButton() => GameManager.UI.ShowPopup<UI_Option>();
    public void ExitButton() => Application.Quit();
}
