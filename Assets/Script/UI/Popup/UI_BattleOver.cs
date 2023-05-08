using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_BattleOver : UI_Scene
{
    [SerializeField] private Image _textImage;
    [SerializeField] private FadeController fc;

    private int _result;

    public void SetImage(int result)
    {
        fc.GetComponent<FadeController>().StartFadeIn();

        GetComponent<Canvas>().sortingOrder = 100;
        _result = result;

        if (result == 1) 
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/WinText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("UI/ResultSFX/승리 효과음");
        }
        else if (result == 2)
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/EliteWinText");
        }
        else if (result == 3) 
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/LoseText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("UI/ResultSFX/패배 효과음");
        }
    }

    public void OnClick()
    {
        if (_result == 3)
            SceneChanger.SceneChange("MainScene");
        else
            SceneChanger.SceneChange("StageSelectScene");
    }
}