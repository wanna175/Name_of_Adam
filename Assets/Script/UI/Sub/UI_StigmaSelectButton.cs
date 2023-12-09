using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_StigmaSelectButton : UI_Base
{
    [SerializeReference] private Image _stigmaImage;
    [SerializeReference] private TextMeshProUGUI _stigmaName;
    [SerializeReference] private TextMeshProUGUI _stigmaDescription;

    private StigmaSceneController _sc;
    private UI_StigmaSelectButtonPopup _popup;
    private Stigma _stigma;

    public void Init(Stigma stigma, StigmaSceneController sc = null, UI_StigmaSelectButtonPopup popup = null)
    {
        _popup = popup;
        _sc = sc;
        _stigma = stigma;

        _stigmaName.text = stigma.Name;
        _stigmaDescription.text = stigma.Description;
        _stigmaImage.sprite = stigma.Sprite;
    }

    public void OnClick()
    {
        //if()
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        if (_sc != null)
        {
            _sc.OnStigmaSelected(_stigma);
        }
        else if(_popup != null)
        {
            _popup.OnClick(_stigma);
        }


        //sc.OnStigmaSelected(_stigma);
    }
}
