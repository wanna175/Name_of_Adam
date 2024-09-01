using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SystemInfo : UI_Popup
{
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private TMP_Text _tooltipText;
    [SerializeField] private Button _confirmButton;

    public void Init(string info, string tooltip)
    {
        _infoText.SetText(GameManager.Locale.GetLocalizedSystem(info));
        _tooltipText.SetText(GameManager.Locale.GetLocalizedSystem(tooltip));
        _confirmButton.onClick.AddListener(() => OnDefaultEvent());
        _confirmButton.onClick.AddListener(() => OnDefaultEvent());
    }

    public void Init(string info, string tooltip, Action OnConfirm)
    {
        Init(info, tooltip);

        _confirmButton.onClick.AddListener(() => OnConfirm());
    }

    private void OnDefaultEvent()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        GameManager.UI.ClosePopup(this);
    }
}
