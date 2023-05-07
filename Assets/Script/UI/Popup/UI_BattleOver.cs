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
        if (result == 1)
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/WinText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("UI/ResultSFX/�¸� ȿ���");
        }
        else if (result== 2)
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/EliteWinText");
        }
        else if (result== 3) 
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/LoseText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("UI/ResultSFX/�й� ȿ���");
        }
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