using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_StigmaSelectButton : UI_Popup
{
    [SerializeReference] private List<UI_HoverImageBlock> _image;
    [SerializeReference] private List<TextMeshProUGUI> _text;

    private StigmaStore _store;
    public void init(StigmaStore store,  List<³«ÀÎ> stigmaList)
    {
        _store = store;

        for (int i = 0; i < 3; i++)
            _text[i].text = stigmaList[i].ToString();

        DeckUnit unit = new();

        for (int i = 0; i < 3; i++)
            _image[i].Set(unit.GetStigmaImage(stigmaList[i]), unit.GetStigmaText(stigmaList[i]));
    }

    public void OnClick(int select)
    {
        _store.OnStigmaSelect(select);
        GameManager.UI.CloseHover();
    }
}
