using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_BattleOver : UI_Popup
{
    [SerializeField] private Image _textImage;

    private int _result;

    public void SetImage(int result)
    {
        _result = result;

        if (result == 1)
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/WinText");
        else if (result == 2)
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/EliteWinText");
        else if (result == 3) 
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/LoseText");

    }

    public void OnClick()
    {
        GameManager.UI.ClosePopup();
        if (_result == 3)
            SceneChanger.SceneChange("MainScene");
        else
            SceneChanger.SceneChange("StageSelectScene");
    }
}