using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_SystemSelect : UI_Popup
{
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    public void Init(string info, Action OnYesButton)
    {
        _infoText.SetText(GameManager.Locale.GetLocalizedSystem(info));
        _yesButton.onClick.AddListener(() => OnDefaultEvent());
        _noButton.onClick.AddListener(() => OnDefaultEvent());
        _yesButton.onClick.AddListener(() => OnYesButton());
    }

    public void Init(string info, Action OnYesButton, Action OnNoButton)
    {
        Init(info, OnYesButton);

        _noButton.onClick.AddListener(() => OnNoButton());
    }

    private void OnDefaultEvent()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.ClosePopup(this);
    }
}
