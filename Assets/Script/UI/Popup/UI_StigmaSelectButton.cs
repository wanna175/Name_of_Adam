using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_StigmaSelectButton : UI_Popup
{
    private StigmaStore _ss;
    public void init(StigmaStore ss)
    {
        _ss = ss; 
    }

    public void OnClick(int select)
    {
        _ss.OnStigmaSelect(select);
    }
}
