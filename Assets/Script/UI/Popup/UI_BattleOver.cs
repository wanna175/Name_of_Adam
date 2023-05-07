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
            GameManager.Sound.Clear();
            GameManager.Sound.Play("UI/ResultSFX/승리 효과음");
        }
        else if (imageNum == 2)
        {
            _textImage.sprite = _eliteWinText;
        }
        else if (imageNum == 3) 
        {
            _textImage.sprite = _loseText;
            GameManager.Sound.Clear();
            GameManager.Sound.Play("UI/ResultSFX/패배 효과음");
        }
    }

    public void OnClick()
    {
        GameManager.UI.ClosePopup();
        SceneChanger.SceneChange("StageSelectScene");
    }
}