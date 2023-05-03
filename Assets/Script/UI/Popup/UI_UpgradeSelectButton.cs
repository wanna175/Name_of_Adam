using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_UpgradeSelectButton : UI_Popup
{
    private UpgradeStore _us;
    public void init(UpgradeStore us)
    {
        _us = us; 
    }

    public void OnClick(int select)
    {
        _us.OnSelect(select);
    }
}
