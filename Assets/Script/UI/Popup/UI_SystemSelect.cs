using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using System;

public class UI_SystemSelect : UI_Popup
{
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    public void Init(string info, Action OnYesButton)
    {
        _infoText.SetText(GameManager.Locale.GetLocalizedSystem(info));
        _yesButton.onClick.AddListener(() => OnYesButton());
        _yesButton.onClick.AddListener(() => ButtonSound());
        _noButton.onClick.AddListener(() => GameManager.UI.ClosePopup(this));
        _noButton.onClick.AddListener(() => ButtonSound());
    }

    public void Init(string info, Action OnYesButton, Action OnNoButton)
    {
        Init(info, OnYesButton);

        _noButton.onClick.AddListener(() => OnNoButton());
    }

    private void ButtonSound() => GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
}
