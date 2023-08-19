using System;
using UnityEngine;

public class UI_ESCOption : UI_Popup
{
    public void QuitButton() => GameManager.UI.ClosePopup();
    public void OptionButton() => GameManager.UI.ShowPopup<UI_Option>();
    public void ExitButton() => Application.Quit();
}
