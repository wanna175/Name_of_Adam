using UnityEngine;

public class UI_SettingButton : UI_Scene
{
    public void OnSettingButtonClick()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        GameManager.UI.OnOffESCOption();
    }
}