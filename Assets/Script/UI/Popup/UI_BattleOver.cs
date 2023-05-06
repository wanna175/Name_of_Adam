using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_BattleOver : UI_Popup
{
    [SerializeField] private Image _textImage;
    [SerializeField] private Sprite _winText;
    [SerializeField] private Sprite _eliteWinText;
    [SerializeField] private Sprite _loseText;

    public void SetImage(int imageNum)
    {
        if (imageNum == 1)
        {
            _textImage.sprite = _winText;
        }
        else if (imageNum == 2)
        {
            _textImage.sprite = _eliteWinText;
        }
        else if (imageNum == 3) 
        {
            _textImage.sprite = _loseText;
        }
    }

    public void OnClick()
    {
        GameManager.UI.ClosePopup();
        SceneChanger.SceneChange("StageSelectScene");
    }
}