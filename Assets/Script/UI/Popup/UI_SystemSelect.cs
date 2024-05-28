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
        _yesButton.onClick.AddListener(() => OnDefaultYesEvent());
        _noButton.onClick.AddListener(() => OnDefaultNoEvent());
        _yesButton.onClick.AddListener(() => OnYesButton());
    }

    public void Init(string info, Action OnYesButton, Action OnNoButton)
    {
        Init(info, OnYesButton);

        _noButton.onClick.AddListener(() => OnNoButton());
    }

    private void OnDefaultYesEvent()
    {
        // YesButton은 클릭 효과음이 다르기 때문에 직접 이벤트로 추가
        GameManager.UI.ClosePopup(this);
    }

    private void OnDefaultNoEvent()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        GameManager.UI.ClosePopup(this);
    }
}
