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
    [SerializeReference] private GameObject _normalFrame;
    [SerializeReference] private GameObject _goldFrame;
    [SerializeReference] private GameObject _redFrame;

    private UI_StigmaSelectButtonPopup _popup;
    private Stigma _stigma;

    public void Init(Stigma stigma, UI_StigmaSelectButtonPopup popup = null)
    {
        _popup = popup;
        _stigma = stigma;

        _stigmaName.text = stigma.Name;
        _stigmaDescription.text = stigma.Description;
        _stigmaImage.sprite = stigma.Sprite_164;

        _normalFrame.SetActive(stigma.Tier == StigmaTier.Tier1);
        _goldFrame.SetActive(stigma.Tier != StigmaTier.Tier1 && stigma.Tier != StigmaTier.Harlot);
        _redFrame.SetActive(stigma.Tier == StigmaTier.Harlot);
    }

    public void OnClick()
    {
        GameManager.Sound.Play("UI/UISFX/UISelectSFX");
        _popup.OnClickStigmataButton(_stigma);
    }
}
