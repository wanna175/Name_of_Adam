using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_StigmaSelectButton : UI_Popup
{
    [SerializeReference] private Image _imageBTN1;
    [SerializeReference] private Image _imageBTN2;
    [SerializeReference] private Image _imageBTN3;

    [SerializeReference] private TextMeshProUGUI _textBTN1;
    [SerializeReference] private TextMeshProUGUI _textBTN2;
    [SerializeReference] private TextMeshProUGUI _textBTN3;

    private StigmaStore _ss;
    public void init(StigmaStore ss,  List<³«ÀÎ> stigmaList)
    {
        _ss = ss;
        
        _textBTN1.text = stigmaList[0].ToString();
        _textBTN2.text = stigmaList[1].ToString();
        _textBTN3.text = stigmaList[2].ToString();

        DeckUnit u = new();

        _imageBTN1.sprite = u.GetStigmaImage(stigmaList[0]);
        _imageBTN2.sprite = u.GetStigmaImage(stigmaList[1]);
        _imageBTN3.sprite = u.GetStigmaImage(stigmaList[2]);

    }

    public void OnClick(int select)
    {
        _ss.OnStigmaSelect(select);
    }
}
