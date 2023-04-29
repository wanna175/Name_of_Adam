using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OptionButton : UI_Scene
{
    public void OnOptionButtonClick()
    {
        GameManager.UI.ShowPopup<UI_Option>();
    }
}