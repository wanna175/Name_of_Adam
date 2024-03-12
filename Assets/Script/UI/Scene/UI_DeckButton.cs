using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeckButton : UI_Scene
{
    public void OnDeckButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(true);
    }
}