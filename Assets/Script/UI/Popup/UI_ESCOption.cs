using System;
using UnityEngine;

public class UI_ESCOption : UI_Popup
{
    // 버튼 Hover 전용 이펙트 필요?

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
        // 메인으로 돌아갈 때 저장 필요?
    }

    public void ExitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        Application.Quit();
        // 게임 나갈 때 저장 필요?
    }
}
